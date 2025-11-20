import { Component, Input } from '@angular/core';
import { NgSwitch, NgSwitchCase, NgSwitchDefault } from '@angular/common';

export type IconName =
  | 'dashboard'
  | 'users'
  | 'documents'
  | 'photos'
  | 'hierarchy'
  | 'message'
  | 'help'
  | 'settings'
  | 'filter';

@Component({
  selector: 'app-icon',
  standalone: true,
  imports: [NgSwitch, NgSwitchCase, NgSwitchDefault],
  templateUrl: './icon.component.html',
  styleUrls: ['./icon.component.css'],
})
export class IconComponent {
  @Input({ required: true }) name!: IconName;
}
