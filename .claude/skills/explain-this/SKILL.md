---
name: explain-this
description: Tłumaczy wskazany fragment kodu WordWise (plik, klasę, metodę lub zakres linii) krok po kroku, z nazwami koncepcji i linkami do dokumentacji. Używaj, gdy autor pyta "co tu się dzieje", "wytłumacz ten plik/fragment".
---

# Wyjaśnij fragment kodu

Argument: ścieżka pliku, opcjonalnie `:od-do` linii lub nazwa symbolu.

1. Przeczytaj tylko wskazany fragment + bezpośrednie zależności potrzebne do zrozumienia.
2. Odpowiedz w strukturze:
   - **Co robi** (1–2 zdania, poziom „biznesowy” — np. „zwraca karty do nauki”).
   - **Linia po linii / blok po bloku** — co i *dlaczego*; nazwij każdą koncepcję (np. *primary constructor*, *expression-bodied member*, *LINQ deferred execution*).
   - **Analogia do TS/Angular**, jeśli pomaga.
   - **Pułapki** i co by się stało przy typowej zmianie (np. usunięciu `AsNoTracking`).
   - **Docs** — 1–3 linki z MCP `microsoft-learn`.
   - **Sprawdź się** — 1 pytanie.
3. Nie modyfikuj kodu.
