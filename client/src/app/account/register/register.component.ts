import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styles: [
  ]
})
export class RegisterComponent implements OnInit {
  userForms: FormArray = this.fb.array([]);

  constructor(private fb: FormBuilder,
              private accountService: AccountService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.userForms.push(this.fb.group({
      displayName: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(20)]],
      email: [null,
        [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],
      ],
      password: ['', [Validators.required,
        Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.compareValues('password')]]
    }));
  }

  private compareValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null : {isEqual: true};
    };
  }

  recordSubmit(fg: FormGroup) {
    this.accountService.register(fg.value).subscribe(
      (res: any) => {
        this.toastr.success('Confirmation link has been sent to your mail');
      }, error => {
          console.log(error);
        });
      }
}
