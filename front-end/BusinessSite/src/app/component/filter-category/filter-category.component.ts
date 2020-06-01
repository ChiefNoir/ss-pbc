import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-filter-category',
  templateUrl: './filter-category.component.html',
  styleUrls: ['./filter-category.component.scss']
})

export class FilterCategoryComponent {
  @Input()
  public navigations: Array<INavigation>;
}
