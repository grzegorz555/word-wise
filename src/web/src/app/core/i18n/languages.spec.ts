import { isLanguageCode } from './languages';

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
