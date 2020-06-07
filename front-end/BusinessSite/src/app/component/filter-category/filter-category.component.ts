import { Component, Input } from '@angular/core';
import { Category } from 'src/app/model/Category';

@Component({
  selector: 'app-filter-category',
  templateUrl: './filter-category.component.html',
  styleUrls: ['./filter-category.component.scss']
})

export class FilterCategoryComponent {
  @Input()
  public categories: Array<Category>;
}
