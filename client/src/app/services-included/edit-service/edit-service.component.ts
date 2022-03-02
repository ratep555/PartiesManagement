import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { ServiceIncluded } from 'src/app/shared/models/birthdays/serviceincluded';
import { ServicesIncludedService } from '../services-included.service';

@Component({
  selector: 'app-edit-service',
  templateUrl: './edit-service.component.html',
  styleUrls: ['./edit-service.component.css']
})
export class EditServiceComponent implements OnInit {
  serviceIncludedForm: FormGroup;
  id: number;
  serviceIncluded: ServiceIncluded;


  constructor(public servicesIncludedService: ServicesIncludedService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.loadServiceIncluded();

    this.serviceIncludedForm = this.fb.group({
      id: [this.id],
      name: [null, [Validators.required]],
      videoClip: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      picture: ''
    });

    this.servicesIncludedService.getServiceIncludedById(this.id)
    .pipe(first())
    .subscribe(x => this.serviceIncludedForm.patchValue(x));
  }

  loadServiceIncluded() {
    return this.servicesIncludedService.getServiceIncludedById(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.serviceIncluded = response;
    }, error => {
    console.log(error);
    });
    }


  onSubmit() {
    this.servicesIncludedService.updateServiceIncluded(this.id, this.serviceIncludedForm.value).subscribe(() => {
    this.router.navigateByUrl('services');
        }, error => {
          console.log(error);
        });
      }

      onImageSelected(image){
        this.serviceIncludedForm.get('picture').setValue(image);
      }

}
