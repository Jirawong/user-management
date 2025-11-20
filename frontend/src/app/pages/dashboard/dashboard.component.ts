import { Component, OnInit } from '@angular/core';
import { UserApiService, UserListItem } from '../../services/user-api.services';
import { FormsModule } from '@angular/forms';
import { DropdownComponent } from '../../shared/dropdown/dropdown.component';
import { CommonModule, NgClass, NgFor, NgIf } from '@angular/common';

interface DashboardUserRow {
  id: string;
  name: string;
  email: string;
  permission: string;
  createdAt: string;
  role: string;
  raw: UserListItem;
}

interface RoleDto {
  roleId: string;
  roleName: string;
}

interface PermissionInput {
  permissionId: string;
  isReadable: boolean;
  isWriteable: boolean;
  isDeleteable: boolean;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, NgFor, NgIf, NgClass, FormsModule, DropdownComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  users: DashboardUserRow[] = [];

  loading = false;
  page = 1;
  pageSize = 10;
  totalCount = 0;

  search: string = '';
  sortBy: string = 'createDate';
  sortDirection: string = 'desc';

  onFiltersChanged() {
    this.page = 1;
    this.loadUsers();
  }

  // modal state
  showAddUserModal = false;
  roles: RoleDto[] = [];
  modulePermissions = [
    { name: 'Super Admin', read: true, write: true, delete: true },
    { name: 'Admin', read: true, write: true, delete: false },
    { name: 'Employee', read: true, write: false, delete: false },
  ];

  constructor(private api: UserApiService) {}

  ngOnInit(): void {
    this.loadUsers();
    this.loadRoles();
  }

  // === API CALL ===
  loadUsers() {
    this.loading = true;

    this.api
      .getUsers({
        orderBy: this.sortBy,
        orderDirection: this.sortDirection,
        pageNumber: this.page,
        pageSize: this.pageSize,
        search: this.search,
      })
      .subscribe({
        next: (res: { totalCount: number; dataSource: any[] }) => {
          this.totalCount = res.totalCount;
          console.log('RAW RES:', res); // <- LOOK HERE
          debugger;
          this.users = res.dataSource.map((u) => ({
            id: u.userId,
            name: `${u.firstName} ${u.lastName}`,
            email: u.email,
            createdAt: new Date(u.createDate).toLocaleDateString(),
            role: u.role.roleName,
            permission: u.permissions?.permissionName ?? 'Unknown',
            raw: u,
          }));

          this.loading = false;
        },
        error: (err: any) => {
          console.error('Failed to load users', err);
          this.loading = false;
        },
      });
  }

  loadRoles() {
    this.api.getRoles().subscribe({
      next: (res) => {
        this.roles = res.data;
      },
      error: (err) => {
        console.error('Failed to load roles', err);
      },
    });
  }

  deleteUser(id: string) {
    if (!confirm('Are you sure you want to delete this user?')) return;

    this.api.deleteUser(id).subscribe({
      next: (res) => {
        console.log('Delete success', res);

        this.loadUsers();
      },
      error: (err) => {
        console.error('Delete failed', err);
      },
    });
  }

  // SEARCH
  onSearchChange(value: string) {
    this.search = value;
    this.page = 1;
    this.loadUsers();
  }

  // SORT DROPDOWN
  onSortChange(option: string) {
    if (option === 'Sort by: Recent' || option === 'Recent') {
      this.sortBy = 'createDate';
      this.sortDirection = 'desc';
    } else if (option === 'Sort by: Name' || option === 'Name') {
      this.sortBy = 'firstName';
      this.sortDirection = 'asc';
    } else if (option === 'Sort by: Role' || option === 'Role') {
      this.sortBy = 'roleName';
      this.sortDirection = 'asc';
    }
    this.loadUsers();
  }

  // ITEMS PER PAGE
  onPageSizeChange(value: string) {
    const n = Number(value);
    if (!isNaN(n) && n > 0) {
      this.pageSize = n;
      this.page = 1;
      this.loadUsers();
    }
  }

  // PAGINATION BUTTONS
  prevPage() {
    if (this.page <= 1) return;
    this.page--;
    this.loadUsers();
  }

  nextPage() {
    if (this.page * this.pageSize >= this.totalCount) return;
    this.page++;
    this.loadUsers();
  }

  newUser = {
    userId: '',
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    roleId: '',
    username: '',
    password: '',
    confirmPassword: '',
    permissions: [] as {
      permissionId: string;
      isReadable: boolean;
      isWriteable: boolean;
      isDeleteable: boolean;
    }[],
  };

  // MODAL TOGGLE
  openAddUser() {
    this.showAddUserModal = true;
  }

  closeAddUser() {
    this.showAddUserModal = false;
  }

  private buildPermissions(): PermissionInput[] {
    // simple dummy: tie permissionId to roleId, allow full access
    return [
      {
        permissionId: this.newUser.roleId,
        isReadable: true,
        isWriteable: true,
        isDeleteable: false,
      },
    ];
  }
  onSaveUser() {
    if (
      !this.newUser.firstName ||
      !this.newUser.lastName ||
      !this.newUser.email ||
      !this.newUser.roleId ||
      !this.newUser.username ||
      !this.newUser.password
    ) {
      alert('Please fill all required fields (*)');
      return;
    }

    if (this.newUser.password !== this.newUser.confirmPassword) {
      alert('Password and Confirm password do not match');
      return;
    }

    const payload = {
      id: this.newUser.userId, // backend ignores / can validate
      firstName: this.newUser.firstName,
      lastName: this.newUser.lastName,
      email: this.newUser.email,
      phone: this.newUser.phone,
      roleId: this.newUser.roleId,
      username: this.newUser.username,
      password: this.newUser.password,
      permissions: this.buildPermissions(),
    };

    console.log('Create user payload:', payload);

    this.api.createUser(payload).subscribe({
      next: (res) => {
        console.log('Create success', res);
        this.loadUsers();
        this.closeAddUser();
      },
      error: (err) => {
        console.error('Create failed', err);
        alert('Failed to create user, check console');
      },
    });
  }
}
