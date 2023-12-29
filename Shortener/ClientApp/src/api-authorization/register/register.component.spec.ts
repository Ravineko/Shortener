import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { RegisterComponent } from './register.component';
import { ActivatedRoute, ActivatedRouteSnapshot, UrlSegment, convertToParamMap, Params, Router } from '@angular/router';
import { of } from 'rxjs';
import { LoginActions } from '../api-authorization.constants';
import { HttpParams } from '@angular/common/http';
import { AuthorizeService } from '../authorize.service';
import { HomeComponent } from 'src/app/home/home.component';

class RouterStub {
  url = '';
  navigate(commands: any[], extras?: any) {}
}

describe('LoginComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let router: Router;

  beforeEach(async(() => {
    let tempParams: Params = { id: '1234' };

    let segment0: UrlSegment = new UrlSegment('segment0', {});
    let segment1: UrlSegment = new UrlSegment(LoginActions.Login, {});

    let urlSegments: UrlSegment[] = [segment0, segment1];

    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule.withRoutes([
        { path: 'authentication/login-failed', component: HomeComponent }
      ])],
      declarations: [RegisterComponent, HomeComponent],
      providers: [{
        provide: ActivatedRoute, useValue: {
          snapshot: {
            paramMap: convertToParamMap(tempParams),
            url: urlSegments,
            queryParams: tempParams
          }
        }
      }]
    }).compileComponents();

    router = TestBed.get(Router);
    spyOn(router, 'navigate').and.returnValue(Promise.resolve(true));
  }));

  beforeEach(() => {
    let authService = TestBed.get(AuthorizeService);

    spyOn(authService, 'ensureUserManagerInitialized').and.returnValue(
      Promise.resolve());

    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
