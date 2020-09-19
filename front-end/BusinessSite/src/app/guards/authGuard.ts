import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { Injectable} from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Identity } from '../model/Identity';
import { AuthService } from '../service/auth.service';
import { StorageService } from '../service/storage.service';
import { Account } from 'src/app/model/Account';
import { RequestResult } from '../model/RequestResult';

@Injectable({
  providedIn: 'root',
})

export class AuthGuard implements CanActivate
{
  private authService: AuthService;
  private router: Router;
  private storageService: StorageService;

  public account: Account;
  public isLoggedIn$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public validating$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  public constructor(authService: AuthService, router: Router, storage: StorageService)
  {
    this.authService = authService;
    this.router = router;
    this.storageService = storage;

    this.checkIsLogged();
  }

  public async checkIsLogged(): Promise<void>
  {
    if(this.validating$.value === true) return;

    this.validating$.next(true);

    const token = this.storageService.getToken();
    if (!token)
    {
      this.logoutComplete();
      return;
    }

    await this.authService
              .validate(token)
              .then
              (
                win => this.handleIdentity(win),
                fail => this.handleError(fail)
              );
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

  private handleIdentity(result: RequestResult<Identity>): void
  {
    if (result.isSucceed)
    {
      this.loginComplete(result.data);
    }
    else
    {
      this.handleError(result.error);
    }
  }

  private handleError(error: any): void
  {
    console.log(error);
    this.logoutComplete();
  }

}
