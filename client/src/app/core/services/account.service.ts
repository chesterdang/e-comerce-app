import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Address, User } from '../../shared/models/user';
import { map, tap } from 'rxjs';
import { SignalrService } from './signalr.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  private signalrService = inject(SignalrService);
  currentUser = signal<User | null>(null);

  login(value?: any) {
    let params = new HttpParams();
    params = params.append('useCookies',true);
    return this.http.post<User>(this.baseUrl + 'login', value, {params}).pipe(
      tap(() => this.signalrService.stopHubConnection())
    );
  }

  register(value? : any) {
    return this.http.post(this.baseUrl + 'account/register', value);
  }

  getInfor() {
    return this.http.get<User>(this.baseUrl + 'account/user-info').pipe(
      map(user => {
        this.currentUser.set(user);
        return user;
      })
    )

  }
  logout() {
    return this.http.post(this.baseUrl + 'account/logout',{});
  }

  updateAddress(address: Address) {
    return this.http.post<Address>(this.baseUrl + 'account/address', address).pipe(
      tap(() => {
        this.currentUser.update(user => {
          if (user) user.address = address;
          return user;
        })
      })
    );
  }
  getAuthState() {
    return this.http.get<{isAuthenticated: boolean}>(this.baseUrl + 'account/auth-status');
  }
}
