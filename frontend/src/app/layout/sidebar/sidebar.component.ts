import { Component } from '@angular/core';
import { NgClass } from '@angular/common';
import { IconComponent } from '../../shared/icon/icon.component';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [NgClass, IconComponent],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent {
  active = 'dashboard';

  setActive(name: string) {
    this.active = name;
  }
}
