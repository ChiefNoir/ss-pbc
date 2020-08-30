import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { AuthService } from '../service/auth.service';
import { StorageService } from '../service/storage.service';
import { BehaviorSubject } from 'rxjs';
import {Account} from 'src/app/model/Account';

@Injectable({
  providedIn: 'root',
})

export class AuthGuard implements CanActivate
{
  private authService: AuthService;
  private storage: StorageService;
  private router: Router;

  public isLoggedIn$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);


  public account: Account;

  public constructor(authService: AuthService, storage: StorageService, router: Router)
  {
    console.log('AuthGuard: constructor');

    this.authService = authService;
    this.storage = storage;
    this.router = router;

    this.isLoggedIn$.next(true);
    //this.deepCheck();
  }

  public canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<UrlTree | boolean> | UrlTree
  {
    if (!this.storage.getToken()) { return this.router.parseUrl('/login'); }

    return this.authService
               .validate(this.storage.getToken())
               .then
               (
                 ok =>
                 {
                   if(ok.data) return true;
                   return this.router.parseUrl('/login');
                },
                notok => false
      );
  }


  public canSee(routerLink: string): boolean
  {

    if (!this.deepCheck()) { return false; }

    if (routerLink === '/admin/editor/projects/') { return this.account.role === 'admin' || this.account.role === 'demo' ; }
    if (routerLink === '/admin/editor/categories/') { return this.account.role === 'admin' || this.account.role === 'demo' ; }
    if (routerLink === '/admin/editor/introduction/') { return this.account.role === 'admin' || this.account.role === 'demo' ; }
    if (routerLink === '/admin/') { return this.account.role === 'admin' || this.account.role === 'demo' ; }

    if (routerLink === '/admin/editor/accounts/') { return this.account.role === 'admin'; }

    return false;
  }

  public loginComplete(account: Account): void
  {
    console.log(account);

    this.isLoggedIn$.next(true);
    this.account = account;
  }

  public logoutComplete(): void
  {
    this.isLoggedIn$.next(false);
    this.account = null;
  }

  private deepCheck(): boolean
  {
    if(this.isLoggedIn$.value) return true;
    if(this.storage.getToken()) 
    {
      this.isLoggedIn$.next(true);
      return true;
    }


    return false;
  }
}
