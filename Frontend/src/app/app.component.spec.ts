import { TestBed, async } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ParityComponent } from './components/parityOfTheWeek/parityOfTheWeek.component';

describe('ParityComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule
      ],
      declarations: [
        ParityComponent
      ],
    }).compileComponents();
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(ParityComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'Frontend'`, () => {
    const fixture = TestBed.createComponent(ParityComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app.title).toEqual('Frontend');
  });

  it('should render title in a h1 tag', () => {
    const fixture = TestBed.createComponent(ParityComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('h1').textContent).toContain('Welcome to Frontend!');
  });
});
