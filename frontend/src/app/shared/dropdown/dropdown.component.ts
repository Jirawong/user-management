import { Component, Input } from '@angular/core';
import { NgIf, NgForOf } from '@angular/common';
import { IconComponent, IconName } from '../icon/icon.component';

@Component({
  selector: 'app-dropdown',
  standalone: true,
  imports: [NgIf, NgForOf, IconComponent],
  templateUrl: './dropdown.component.html',
  styleUrls: ['./dropdown.component.css'],
})
export class DropdownComponent {
  @Input() icon: IconName = 'filter';
  @Input() items: string[] = [];

  open = false;

  toggle() {
    this.open = !this.open;
  }

  selectItem(item: string) {
    console.log('Selected:', item);
    this.open = false;
  }
}
