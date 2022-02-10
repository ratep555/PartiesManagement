import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ItemCreateEdit } from 'src/app/shared/models/item';

@Component({
  selector: 'app-form-item-felipe',
  templateUrl: './form-item-felipe.component.html',
  styleUrls: ['./form-item-felipe.component.css']
})
export class FormItemFelipeComponent implements OnInit {

  constructor(private fb: FormBuilder) { }
  form: FormGroup;

  @Input() model: ItemCreateEdit;

  @Output() onSaveChanges = new EventEmitter<ItemCreateEdit>();


  ngOnInit(): void {
    this.form = this.fb.group({
      name: [null, [Validators.required]],
      price: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      picture: ''
    });

    if (this.model !== undefined){
      this.form.patchValue(this.model);
    }
  }

  onImageSelected(image){
    this.form.get('picture').setValue(image);
  }

  saveChanges(){
    this.onSaveChanges.emit(this.form.value);
  }


}








