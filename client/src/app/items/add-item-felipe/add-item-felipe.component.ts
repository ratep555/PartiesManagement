import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ItemCreateEdit } from 'src/app/shared/models/item';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { ItemsService } from '../items.service';

@Component({
  selector: 'app-add-item-felipe',
  templateUrl: './add-item-felipe.component.html',
  styleUrls: ['./add-item-felipe.component.css']
})
export class AddItemFelipeComponent implements OnInit {
  nonSelectedCategories: MultipleSelectorModel[];
  nonSelectedManufacturers: MultipleSelectorModel[];
  nonSelectedTags: MultipleSelectorModel[];

  constructor(private itemsService: ItemsService,
              private router: Router) { }

  ngOnInit(): void {
  }

  saveChanges(item: ItemCreateEdit){
    this.itemsService.createItem1(item).subscribe(() => {
      this.router.navigate(['/items']);
    });
  }

}
