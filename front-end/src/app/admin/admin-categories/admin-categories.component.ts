import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

import { environment } from 'src/environments/environment';
import { PublicService } from '../../core/public.service';
import { RequestResult, Incident } from '../../shared/request-result.model';
import { Category } from '../../shared/category.model';
import { MatDialog } from '@angular/material/dialog';
import { DialogEditorCategoryComponent } from '../dialog-editor-category/dialog-editor-category.component';
import {
  MessageType,
  MessageDescription,
} from '../../shared/message/message.component';
import { ResourcesService } from '../../core/resources.service';
import { AuthGuard } from '../../core/auth.guard';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-categories',
  templateUrl: './admin-categories.component.html',
  styleUrls: ['./admin-categories.component.scss'],
})
export class AdminCategoriesComponent implements OnInit {
  public categories$: BehaviorSubject<Array<Category>> = new BehaviorSubject<
    Array<Category>
  >(null);
  public message$: BehaviorSubject<MessageDescription> = new BehaviorSubject<
    MessageDescription
  >({ type: MessageType.Spinner });

  private columnDefinitions = [
    { def: 'id', show: false },
    { def: 'code', show: true },
    { def: 'displayName', show: true },
    { def: 'isEverything', show: true },
  ];

  public constructor(
    private service: PublicService,
    public textMessages: ResourcesService,
    titleService: Title,
    public dialog: MatDialog,
    private authGuard: AuthGuard,
    private router: Router
  ) {
    titleService.setTitle(environment.siteName);
  }

  public ngOnInit(): void {
    if (this.authGuard.isLoggedIn()) {
      this.refreshCategories();
    } else {
      this.router.navigate(['/login']);
    }
  }

  public getDisplayedColumns(): string[] {
    return this.columnDefinitions.filter((x) => x.show).map((x) => x.def);
  }

  public showDialog(categoryId?: number): void {
    const dialogRef = this.dialog.open(DialogEditorCategoryComponent, {
      width: '50%',
      data: categoryId,
    });

    dialogRef
      .afterClosed()
      .toPromise()
      .then(() => this.refreshCategories());
  }

  private refreshCategories(): void {
    // there is no paging in the categories, because there will be not many categories
    this.service.getCategories().then(
      (win) => this.handleCategories(win),
      (fail) => this.handleError(fail)
    );
  }

  private handleCategories(result: RequestResult<Category[]>): void {
    if (result.isSucceed) {
      const sorted = result.data.sort((x, y) => {
        if (x.isEverything < y.isEverything) {
          return 1;
        }

        return -1;
      });

      this.categories$.next(sorted);
    } else {
      this.handleIncident(result.error);
    }
  }

  private handleIncident(error: Incident): void {
    console.log(error);
    this.message$.next({ text: error.message, type: MessageType.Error });
  }

  private handleError(error: any): void {
    console.log(error);

    if (error.name !== undefined) {
      this.message$.next({ text: error.name, type: MessageType.Error });
    } else {
      this.message$.next({ text: error, type: MessageType.Error });
    }
  }
}
