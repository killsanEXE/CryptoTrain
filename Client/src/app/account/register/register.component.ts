import { Component, Input, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  @Input() accountService: AccountService;
  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = this.fb.group({
      name: ["", [Validators.required, this.checkSpaces()]],
      surname: ["", [Validators.required, this.checkSpaces()]],
      email: ["", [Validators.required, this.checkSpaces()]],
      username: ["", [Validators.required, this.checkSpaces()]],
      password: ["", [Validators.required, Validators.minLength(4), Validators.maxLength(6), this.checkSpaces()]],
      passwordConfirm: ["", [Validators.required, this.matchValues()]],
    });

    this.registerForm.controls["password"].valueChanges.subscribe(() => {
      this.registerForm.controls["passwordConfirm"].updateValueAndValidity();
    });
  }

  register(){
    this.accountService.register(this.registerForm.value).subscribe(() => {
      this.snackBar.open("Confirm your email", "Ok");
      this.registerForm.reset();
    })
  }

  matchValues(): ValidatorFn{
    return (control: AbstractControl) => {
      return control?.value === this.registerForm?.controls["password"]?.value ? null : { isMatching: true }
    }
  }

  checkSpaces(): ValidatorFn{
      return (control: AbstractControl) => {
        return String(control?.value).trim() === "" ? { required : true} : null;
      }
  }

}
