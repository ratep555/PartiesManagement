import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { Item } from 'src/app/shared/models/item';
import { WebshopService } from '../webshop.service';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit {
  @Input() item: Item;

  constructor(private basketService: BasketService,
              private webshopService: WebshopService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  addingItemToBasket() {
    this.basketService.addingItemToBasket(this.item);
    this.webshopService.decreaseStockQuantity1(this.item.id, 1).subscribe(() => {
    });
  }

  addLike() {
    this.webshopService.addLike(this.item.id).subscribe(() => {
      this.toastr.success('You have just liked this product!');
    }, error => {
        console.log(error);
        this.toastr.error('Sorry, not authorized!');

      });
    }

}
