import { Component, Input, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Item } from 'src/app/shared/models/item';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit {
  @Input() item: Item;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
  }

  addingItemToBasket() {
    this.basketService.addingItemToBasket(this.item);
  }

}
