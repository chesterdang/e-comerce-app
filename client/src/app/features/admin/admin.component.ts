import { CurrencyPipe, DatePipe } from '@angular/common';
import { AfterViewInit, Component, inject, OnInit, ViewChild } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { RouterLink } from '@angular/router';
import { Order } from '../../shared/models/order';
import { AdminService } from '../../core/services/admin.service';
import { OrderParams } from '../../shared/models/orderParams';
import { MatOption, MatSelect } from '@angular/material/select';
import { MatTab, MatTabGroup } from '@angular/material/tabs';
@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [
    MatTableModule,
    MatPaginatorModule,
    MatIcon,
    MatButton,
    DatePipe,
    MatLabel,
    RouterLink,
    CurrencyPipe,
    MatFormField,
    MatSelect,
    MatOption,
    MatTab,
    MatTabGroup

  ],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})
export class AdminComponent implements  OnInit{
  displayColumns: string[] = ['id', 'buyerEmail', 'orderDate', 'total', 'status', 'actions'];
  dataSource = new MatTableDataSource<Order>([]);
  orderParams = new OrderParams();
  totalItems = 0;
  statusOptions = ['All', 'PaymentReceived', 'PaymentMismatch', 'Refunded', 'Pending']
  private adminService = inject(AdminService);
  
  ngOnInit(): void {
    this.loadOrders();
  }
  loadOrders() {
    this.adminService.getOrders(this.orderParams).subscribe ({
      next: response => {
        if (response.data) {
          this.dataSource.data = response.data;
          this.totalItems = response.count;
        }
      }
    })
  }

  onPageChange(event: any) {
    this.orderParams.pageNumber = event.pageIndex + 1;
    this.orderParams.pageSize = event.pageSize;
    this.loadOrders();
  }

  onFilterSelect(event: any) {
    this.orderParams.pageNumber = 1;
    this.orderParams.filter = event.value;
    this.loadOrders();
  }
  
  refundOrder(id: number) {
    this.adminService.refundOrder(id).subscribe({
      next: order => {
        this.dataSource.data = this.dataSource.data.map(o => o.id === id? order : o)
      }
    })
  }
}
