import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Category } from '../../shared/category.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-filter-category',
  templateUrl: './filter-category.component.html',
  styleUrls: ['./filter-category.component.scss'],
})
export class FilterCategoryComponent implements OnChanges {
  @Input()
  public categories: Array<Category>;

  constructor(private router: Router) {}

  public ngOnChanges(changes: SimpleChanges): void {
    if (!changes.categories.currentValue) {
      return;
    }

    changes.categories.currentValue.forEach((x) => {
      x.url = this.router.createUrlTree(['/projects', x.code]).toString();
    });

    // show only cateroies with entities, in desc order
    this.categories = changes.categories.currentValue
      .filter((x) => x.totalProjects > 0)
      .sort((a, b) => b.totalProjects - a.totalProjects);
  }
}
