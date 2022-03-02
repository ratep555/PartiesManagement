import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ServiceIncluded } from 'src/app/shared/models/birthdays/serviceincluded';
import { ServicesIncludedComponent } from '../services-included.component';
import { ServicesIncludedService } from '../services-included.service';

@Component({
  selector: 'app-add-service',
  templateUrl: './add-service.component.html',
  styleUrls: ['./add-service.component.css']
})
export class AddServiceComponent implements OnInit {
  serviceIncludedForm: FormGroup;
  model: ServiceIncluded;

  constructor(public servicesIncludedService: ServicesIncludedService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.createServiceIncludedForm();
  }

  createServiceIncludedForm() {
    this.serviceIncludedForm = this.fb.group({
      name: [null, [Validators.required]],
      videoClip: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      picture: ''
    });
  }

  onSubmit() {
    this.servicesIncludedService.createServiceIncluded(this.serviceIncludedForm.value).subscribe(() => {
      this.router.navigateByUrl('services');
    },
    error => {
      console.log(error);
    });
  }

  onImageSelected(image){
    this.serviceIncludedForm.get('picture').setValue(image);
  }

}
