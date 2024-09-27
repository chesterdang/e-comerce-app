import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  loading = false;
  busyRequest = 0;
  busy() {
    this.busyRequest++
    this.loading = true
  }
  idle() {
    this.busyRequest--;
    if (this.busyRequest <= 0) {
      this.busyRequest = 0;
      this.loading = false;
    }
  }
}
