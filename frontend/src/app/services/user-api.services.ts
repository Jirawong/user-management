import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Role {
  roleId: string;
  roleName: string;
}

export interface Permission {
  permissionId: string;
  permissionName: string;
}

export interface GetUsersRequest {
  orderBy?: string;
  orderDirection?: string;
  pageNumber?: number;
  pageSize?: number;
  search?: string;
}

export interface UserListItem {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  role: Role;
  username: string;
  permissions: {
    permissionId: string;
    permissionName: string;
  };
  createDate: string;
}

export interface PagedUsersResponse {
  dataSource: UserListItem[];
  page: number;
  pageSize: number;
  totalCount: number;
}

interface RoleDto {
  roleId: string;
  roleName: string;
}

interface RolesResponse {
  status: { code: string; description: string };
  data: RoleDto[];
}
@Injectable({ providedIn: 'root' })
export class UserApiService {
  private base = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  getRoles(): Observable<RolesResponse> {
    return this.http.get<RolesResponse>(`${this.base}/roles`);
  }

  getUsers(req: GetUsersRequest): Observable<PagedUsersResponse> {
    let params = new HttpParams();
    if (req.orderBy) params = params.set('orderBy', req.orderBy);
    if (req.orderDirection)
      params = params.set('orderDirection', req.orderDirection);
    if (req.pageNumber) params = params.set('pageNumber', req.pageNumber);
    if (req.pageSize) params = params.set('pageSize', req.pageSize);
    if (req.search) params = params.set('search', req.search);

    return this.http.get<PagedUsersResponse>(`${this.base}/users`, { params });
  }

  createUser(payload: any) {
    return this.http.post(`${this.base}/users`, payload);
  }

  deleteUser(id: string) {
    return this.http.delete<any>(`${this.base}/users/${id}`);
  }
}
