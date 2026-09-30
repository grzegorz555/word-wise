# Wizja: WordWise

## Problem
Nauka języka od najczęstszych słów daje najszybszy zwrot, bo około 1000 słów pokrywa dużą część codziennych tekstów. Chcę narzędzia, które prowadzi przez te słowa fiszka po fiszce w dowolnej parze języków EN/PL/TR.

## Użytkownik
Osoba ucząca się języka (na start: autor). Ma konto, wybiera język interfejsu, język bazowy i język docelowy.

## Cele projektu
1. **Nauka .NET** (główny cel) i portfolio. Jakość kodu ma być „do pokazania”.
2. Działające MVP w małych, kompletnych krokach (każdy krok: spec → testy → kod → wpis w dzienniku).

## MVP (user stories)
1. Jako użytkownik zakładam konto i loguję się (email + hasło).
2. Jako użytkownik wybieram język interfejsu (PL/EN/TR) i mogę go zmienić w każdej chwili.
3. Jako użytkownik wybieram parę języków (bazowy → docelowy, dowolna para z EN/PL/TR).
4. Jako użytkownik przeglądam fiszki słów docelowych **po kolei** (wg częstości) albo **losowo** i odsłaniam tłumaczenie.
5. Jako użytkownik oznaczam fiszkę jako „znam” / „nie znam”, a aplikacja zapisuje mój postęp.

## Poza MVP (non-goals na teraz) → `backlog.md`
- Algorytm powtórek (SM-2 / FSRS) i ważenie słabszych słów.
- Gry / quizy.
- Pełne listy 1000 słów (w MVP wystarcza seed ~50 słów, patrz ADR-0007).
- Audio, wymowa, przykładowe zdania.
- Hosting publiczny, logowanie zewnętrzne (Google itp.).

## Miary sukcesu
- Każda funkcja MVP ma testy (unit/integration) i przechodzi CI.
- Każda funkcja ma wpis w `learning/journal.md`. Autor umie wyjaśnić każdą warstwę bez pomocy AI.
