import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

import { User } from '../models';

@Injectable()
export class UserService {
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get<User[]>(`${environment.apiUrl}/api/users`);
    }

    getById(id: number) {
        return this.http.get(`${environment.apiUrl}/api/users/` + id);
    }

    register(user: User) {
        return this.http.post(`${environment.apiUrl}/api/users/register`, user);
    }

    update(user: User) {
        return this.http.put(`${environment.apiUrl}/api/users/` + user.id, user);
    }

    delete(id: number) {
        return this.http.delete(`${environment.apiUrl}/api/users/` + id);
    }
}
