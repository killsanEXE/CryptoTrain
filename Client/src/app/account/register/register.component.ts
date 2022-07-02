import { Component, Input, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  @Input() accountService: AccountService;
  registerForm: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = this.fb.group({
      name: ["", Validators.required],
      surname: ["", Validators.required],
      email: ["", Validators.required],
      username: ["", Validators.required],
      password: ["", [Validators.required, Validators.minLength(4), Validators.maxLength(6)]],
      passwordConfirm: ["", [Validators.required, this.matchValues()]],
      // passwordConfirm: ["", [Validators.required]],
    });

    this.registerForm.controls["password"].valueChanges.subscribe(() => {
      this.registerForm.controls["passwordConfirm"].updateValueAndValidity();
    });
  }

  register(){
    console.log(this.registerForm.value);
  }

  matchValues(): ValidatorFn{
    return (control: AbstractControl) => {
      return control?.value === this.registerForm?.controls["password"]?.value ? null : { isMatching: true }
    }
  }

}
