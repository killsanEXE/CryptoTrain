import { ChangeDetectionStrategy, Component, Input, OnInit, Self } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {

  @Input() accountService: AccountService;
  loginForm: FormGroup;

  constructor(private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.loginForm = this.fb.group({
      username: ["", [Validators.required, this.checkForSpaces()]],
      password: ["", [Validators.required]]
    })
  }

  checkForSpaces(): ValidatorFn{
    return (control: AbstractControl) => {
      if(String(control.value).indexOf(" ") >= 0){
        return { withSpaces: true }
      }
      return null;
    }
  }

  login(){
    console.log(this.loginForm.valid);
    console.log(this.loginForm.value);
    // this.accountService.login(this.loginForm.value);
  }
}
