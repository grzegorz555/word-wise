import { TestBed } from '@angular/core/testing';
import { TranslocoTestingModule } from '@jsverse/transloco';
import en from '../../public/i18n/en.json';
import pl from '../../public/i18n/pl.json';
import { App } from './app';

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        App,
        TranslocoTestingModule.forRoot({
          langs: { pl, en },
          translocoConfig: {
            availableLangs: ['pl', 'en', 'tr'],
            defaultLang: 'pl',
            reRenderOnLangChange: true,
          },
          preloadLangs: true,
        }),
      ],
    }).compileComponents();
  });

  it('renders the translated tagline', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('main p')?.textContent).toContain(pl.app.tagline);
  });

  it('switches UI language on click', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    const compiled = fixture.nativeElement as HTMLElement;

    const enButton = [...compiled.querySelectorAll('button')].find(
      (b) => b.textContent?.trim() === 'EN',
    );
    enButton?.click();
    await fixture.whenStable();

    expect(compiled.querySelector('main p')?.textContent).toContain(en.app.tagline);
  });

  it('marks PL as the active language on start', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    const compiled = fixture.nativeElement as HTMLElement;

    const plButton = [...compiled.querySelectorAll('button')].find(
      (b) => b.textContent?.trim() === 'PL',
    );

    expect(plButton?.getAttribute('aria-pressed')).toBe('true');
  });

  // WCAG 2.2 SC 2.5.3 Label in Name: the accessible name starts with the visible text.
  it('starts each language button accessible name with its visible code', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    const compiled = fixture.nativeElement as HTMLElement;

    const buttons = [...compiled.querySelectorAll('button')];
    expect(buttons.map((b) => b.getAttribute('aria-label'))).toEqual([
      'PL, Polski',
      'EN, English',
      'TR, Türkçe',
    ]);
    for (const button of buttons) {
      const visible = button.textContent?.trim() ?? '';
      expect(visible).not.toBe('');
      expect(button.getAttribute('aria-label')?.startsWith(visible)).toBe(true);
    }
  });
});
