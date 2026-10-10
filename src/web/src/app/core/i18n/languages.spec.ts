import { isLanguageCode, languageSwitchLabel } from './languages';

describe('isLanguageCode', () => {
  it.each(['pl', 'en', 'tr'])('returns true for %s', (value) => {
    expect(isLanguageCode(value)).toBe(true);
  });

  it.each(['jp', ' ', null, undefined, [['pl', 'en', 'tr']], 'PL'])(
    'returns false for %s',
    (value) => {
      expect(isLanguageCode(value)).toBe(false);
    },
  );
});

describe('languageSwitchLabel', () => {
  it.each([
    ['pl', 'PL, Polski'],
    ['en', 'EN, English'],
    ['tr', 'TR, Türkçe'],
  ] as const)('puts the visible code first for %s', (lang, expected) => {
    expect(languageSwitchLabel(lang)).toBe(expected);
  });
});
