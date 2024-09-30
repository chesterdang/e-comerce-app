import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { Product } from '../../shared/models/product';
import { Pagination } from '../../shared/models/pagination';
import { MatCard } from '@angular/material/card';
import { ShopService } from '../../core/services/shop.service';
import { ProductItemComponent } from './product-item/product-item.component';
import { FiltersDialogComponent } from "./filters-dialog/filters-dialog.component";
import { ShopParams } from '../../shared/models/shopParams';
import { MatIcon } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { FormsModule } from '@angular/forms';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { RouterLink } from '@angular/router';
import { CartItemComponent } from "../cart/cart-item/cart-item.component";
import { EmptyStateComponent } from '../../shared/components/emty-state/emty-state.component';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    MatCard,
    ProductItemComponent,
    FiltersDialogComponent,
    MatIcon, MatButton, MatListOption,
    FormsModule, MatMenuTrigger,
    MatMenu, MatSelectionList,
    MatListOption, MatPaginator,
    MatIconButton, FormsModule, 
    CartItemComponent, EmptyStateComponent
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit{
  products?: Pagination<Product> ; 
  private dialogService = inject(MatDialog)
  private shopService = inject(ShopService);
  shopParams = new ShopParams();
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low-High', value: 'priceAsc'},
    {name: 'Price: High-Low', value: 'priceDesc'}
  ]

  resetFilters() {
    this.shopParams = new ShopParams();
    this.getProducts();
  }
  ngOnInit(): void {
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.getProducts();
  } 

  OnSearchChange() {
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }
  handlePageEvent(event: PageEvent) {
    this.shopParams.pageNumber = event.pageIndex + 1;
    console.log(event.pageIndex);
    console.log(this.shopParams.pageNumber);
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }

  onSortChange(event: MatSelectionListChange) {
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.shopParams.sort = selectedOption.value;
      this.shopParams.pageNumber = 1;
      this.getProducts();
    }
      
  }
  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: response => {this.products = response as Pagination<Product>;
        console.log(response);
      },
      error: error => console.log(error)
    })
  }
  openFilterDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands: this.shopParams.brands,
        selectedTypes:  this.shopParams.types
      }
    });
    dialogRef.afterClosed().subscribe({
      next: result => {
        if (result) {  
          this.shopParams.brands = result.selectedBrands;
          this.shopParams.types = result.selectedTypes;
          this.shopParams.pageNumber = 1;
          this.getProducts();
        }
      }
    })
  }
}
