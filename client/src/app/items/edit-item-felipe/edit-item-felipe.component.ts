import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Item, ItemCreateEdit } from 'src/app/shared/models/item';
import { ItemsService } from '../items.service';

@Component({
  selector: 'app-edit-item-felipe',
  templateUrl: './edit-item-felipe.component.html',
  styleUrls: ['./edit-item-felipe.component.css']
})
export class EditItemFelipeComponent implements OnInit {
  model: Item;


  constructor(private activatedRoute: ActivatedRoute,
              private itemsService: ItemsService,
              private router: Router) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      this.itemsService.getItemById(params.id).subscribe(item => this.model = item);
    });
  }

  saveChanges(itemcreate: ItemCreateEdit){
    console.log(itemcreate);
    this.itemsService.updateItem1(this.model.id, itemcreate).subscribe(() => {
      this.router.navigate(['/items']);
    });
  }

}








