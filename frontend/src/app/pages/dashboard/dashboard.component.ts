import { Component } from '@angular/core';
import { NgForOf, NgIf } from '@angular/common';
import { NgClass } from '@angular/common';
import { IconComponent } from '../../shared/icon/icon.component';
import { DropdownComponent } from '../../shared/dropdown/dropdown.component';

type Role = 'Admin' | 'User' | 'Viewer';

interface User {
  name: string;
  email: string;
  permission: string;
  createdAt: string;
  role: Role;
}

type PermissionAction = 'read' | 'write' | 'delete';

interface ModulePermission {
  name: string;
  read: boolean;
  write: boolean;
  delete: boolean;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [NgForOf, NgIf, NgClass, IconComponent, DropdownComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent {
  users: User[] = [
    {
      name: 'John Doe',
      email: 'john@example.com',
      permission: 'Super Admin',
      createdAt: '2025-11-18',
      role: 'Admin',
    },
    {
      name: 'Jane Smith',
      email: 'jane@example.com',
      permission: 'HR Admin',
      createdAt: '2025-11-17',
      role: 'User',
    },
    {
      name: 'Alex Johnson',
      email: 'alex@example.com',
      permission: 'Employee',
      createdAt: '2025-11-15',
      role: 'Viewer',
    },
  ];

  modulePermissions: ModulePermission[] = [
    { name: 'Users', read: true, write: true, delete: false },
    { name: 'Documents', read: true, write: false, delete: false },
    { name: 'Photos', read: true, write: true, delete: true },
    { name: 'Hierarchy', read: true, write: false, delete: false },
  ];

  showAddUserModal = false;

  openAddUser() {
    this.showAddUserModal = true;
  }

  closeAddUser() {
    this.showAddUserModal = false;
  }

  onSaveUser() {
    this.showAddUserModal = false;
  }
}
