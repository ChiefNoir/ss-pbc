import { Component } from '@angular/core';
import { ResourcesService } from '../../services/resources.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss'],
})
export class FooterComponent {
  public currentYear: number = new Date().getFullYear();

  constructor(public resourcesService: ResourcesService) {}
}
