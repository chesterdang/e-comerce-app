import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [
    MatButton
  ],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.scss'
})
export class TestErrorComponent {
  baseURL = environment.apiUrl;
  private http = inject(HttpClient);

  get400Error() {
    this.http.get(this.baseURL + 'error/badrequest' ).subscribe({
      next: response => console.log(response),
      error: er => console.log(er)
    })
  }
  get401Error() {
    this.http.get(this.baseURL + 'error/unauthorized' ).subscribe({
      next: response => console.log(response),
      error: er => console.log(er)
    })
  }

  get404Error() {
    this.http.get(this.baseURL + 'error/notfound' ).subscribe({
      next: response => console.log(response),
      error: er => console.log(er)
    })
  }
  get500Error() {
    this.http.get(this.baseURL + 'error/internalerror' ).subscribe({
      next: response => console.log(response),
      error: er => console.log(er)
    })
  }
  get405ValidationError() {
    this.http.get(this.baseURL + 'error/validationerror' ).subscribe({
      next: response => console.log(response),
      error: er => console.log(er)
    })
  }
  

}
