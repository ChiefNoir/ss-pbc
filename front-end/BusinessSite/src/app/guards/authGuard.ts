import { Injectable, OnDestroy, OnInit } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { AuthService } from '../service/auth.service';
import { StorageService } from '../service/storage.service';
import { BehaviorSubject } from 'rxjs';
import {Account} from 'src/app/model/Account';
import { Identity } from '../model/Identity';
import { RequestResult } from '../model/RequestResult';

@Injectable({
  providedIn: 'root',
})

export class AuthGuard implements CanActivate
{
  private authService: AuthService;
  private storageService: StorageService;
  private router: Router;

  public isLoggedIn$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);


  public account: Account;

  public validating$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);

  public constructor(authService: AuthService, storage: StorageService, router: Router)
  {
    this.authService = authService;
    this.storageService = storage;
    this.router = router;

    this.checkIsLogged();
  }

  public async checkIsLogged(): Promise<void>
  {
    this.validating$.next(true);

    const token = this.storageService.getToken();
    

    if (!token)
    {
      this.logoutComplete();
    }
    else
    {
      await this.authService.validate(token)
                      .then
                      (
                        ok => 
                        {
                          if (ok.isSucceed)
    {
      this.loginComplete(ok.data);
    }
    else
    {
      this.logoutComplete();
    }
                        },
                        fail => {console.log(fail); this.logoutComplete();  }
      )
    }
  }

  public canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<UrlTree | boolean> | UrlTree
  {
    if (!this.canSee('/' + next.routeConfig.path))
    {
      return this.router.parseUrl('/login');
    }

    return new Promise(resolve => {
      resolve(true);
    });
  }

  public async canSee(routerLink: string): Promise<boolean>
  {
    if (!this.account) { await this.checkIsLogged(); }
    if (this.account == null || this.validating$.value === true) { return false; }

    if (routerLink === '/admin/editor/projects') { return this.account.role === 'admin' || this.account.role === 'demo' ; }
    if (routerLink === '/admin/editor/categories') { return this.account.role === 'admin' || this.account.role === 'demo' ; }
    if (routerLink === '/admin/editor/introduction') { return this.account.role === 'admin' || this.account.role === 'demo' ; }
    if (routerLink === '/admin') { return this.account.role === 'admin' || this.account.role === 'demo' ; }

    if (routerLink === '/admin/editor/accounts') { return this.account.role === 'admin'; }

    return false;
  }

  public loginComplete(identety: Identity): void
  {
    this.storageService.saveToken(identety.token, identety.tokenLifeTimeMinutes);
    this.isLoggedIn$.next(true);
    this.validating$.next(false);
    this.account = identety.account;
  }

  public logoutComplete(): void
  {
    this.account = null;
    this.isLoggedIn$.next(false);
    this.validating$.next(false);
    this.storageService.removeToken();
  }

}
