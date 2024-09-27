import { Component, inject } from '@angular/core';
import { MatIcon} from '@angular/material/icon';
import { MatButton, MatButtonModule} from '@angular/material/button';
import { MatBadge} from '@angular/material/badge';
import { Router, RouterLink } from '@angular/router';
import { MatProgressBar } from '@angular/material/progress-bar';
import { BusyService } from '../../core/services/busy.service';
import { CartService } from '../../core/services/cart.service';
import { AccountService } from '../../core/services/account.service';
import { MatMenu, MatMenuItem, MatMenuTrigger } from '@angular/material/menu';
import { MatDivider } from '@angular/material/divider';
@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    MatIcon,
    MatBadge,
    MatButton,
    MatButtonModule,
    RouterLink,
    MatProgressBar,
    MatMenuItem,
    MatMenu,
    MatDivider,
    MatMenuTrigger
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  busyService = inject(BusyService);
  cartService = inject(CartService);
  accountService = inject(AccountService);
  private router = inject(Router)
  updateTotalQuantity() :number {

    return this.cartService.itemCount()!;
  }

  logout() {
    this.accountService.logout().subscribe({
      next: () => {
        this.accountService.currentUser.set(null);
        this.router.navigateByUrl("/");
      }
    })
  }
}
