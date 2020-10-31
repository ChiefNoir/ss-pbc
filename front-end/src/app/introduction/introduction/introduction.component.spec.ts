import { TestBed, waitForAsync } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { IntroductionComponent } from './introduction.component';
import { CoreModule } from '../../core/core.module';
import { SharedModule } from '../../shared/shared.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Introduction } from '../../shared/models/introduction.model';
import { PublicService } from '../../core/services/public.service';
import { RequestResult } from '../../shared/models/request-result.interface';
import { HttpClientModule } from '@angular/common/http';
import { of } from 'rxjs';
import { MessageType } from '../../shared/components/message/message.component';
import { Incident } from '../../shared/models/incident.interface';

const mockPublicService = jasmine.createSpyObj(['getIntroduction']);

class MockRequestResult<T> implements RequestResult<T> {
    data: T;
    isSucceed: boolean;
    error: MockIncident;
  }

class MockIncident implements Incident {
    public message: string;
    public detail: string;
}

describe('IntroductionComponent', () => {
    beforeEach(waitForAsync(() => {
        TestBed.configureTestingModule({
            imports: [
                RouterTestingModule,
                BrowserAnimationsModule,
                HttpClientModule,
                CoreModule,
                SharedModule
            ],
            declarations: [
                IntroductionComponent,
            ]
        });

        TestBed.overrideProvider(PublicService, { useValue: mockPublicService });

        TestBed.compileComponents();
    }));

    it('should create', () => {
        const fixture = TestBed.createComponent(IntroductionComponent);
        const component = fixture.componentInstance;

        expect(component).toBeTruthy();
      });

    it('should have spinner', () => {
        const fixture = TestBed.createComponent(IntroductionComponent);
        const component = fixture.componentInstance;

        expect(component.message$.value.type).toEqual(MessageType.Spinner);
    });

    describe('ngOnInit', () => {
        it('should get introduction from info public service', () => {

            let introductionResponse: MockRequestResult<Introduction>;
            introductionResponse = new MockRequestResult<Introduction>();
            introductionResponse.isSucceed = true;
            introductionResponse.data =
            {
                title: 'title',
                externalUrls: null,
                content : 'test',
                posterDescription : 'poster description',
                posterUrl : 'poster url',
                version: 0,
                posterPreview : null,
                posterToUpload : null
            };

            mockPublicService.getIntroduction.and.returnValue(of(introductionResponse));

            const fixture = TestBed.createComponent(IntroductionComponent);
            const component = fixture.componentInstance;

            component.ngOnInit();

            expect(mockPublicService.getIntroduction).toHaveBeenCalled();
            expect(component.introduction$.value).toEqual(introductionResponse.data);
            expect(component.message$.value).toEqual(null);
        });
    });

    describe('ngOnInit', () => {
        it('should react on error from public service', () => {

            let introductionResponse: MockRequestResult<Introduction>;
            introductionResponse = new MockRequestResult<Introduction>();
            introductionResponse.isSucceed = false;
            introductionResponse.error = {message : 'error message', detail : 'error detail'};

            mockPublicService.getIntroduction.and.returnValue(of(introductionResponse));

            const fixture = TestBed.createComponent(IntroductionComponent);
            const component = fixture.componentInstance;

            component.ngOnInit();

            expect(mockPublicService.getIntroduction).toHaveBeenCalled();
            expect(component.introduction$.value).toEqual(null);
            expect(component.message$.value.type).toEqual(MessageType.Error);
            expect(component.message$.value.text).toEqual('error message');
        });
    });

});
