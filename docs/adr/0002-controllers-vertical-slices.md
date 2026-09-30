# ADR-0002: Controllers + vertical slices

- Status: Accepted
- Date: 2026-09-30

## Context
I want to learn an approach used at work, but without the ceremony of multiple layer projects.

## Considered options
1. Minimal API + slices: the least code, less common in existing company projects.
2. **Controllers + slices:** classic MVC, code grouped by feature.
3. Controllers + Clean Architecture (Domain/Application/Infrastructure/Api, MediatR): many patterns, but also a lot of ceremony to start with.

## Decision
Option 2. A single `WordWise.Api` project with `Features/<Feature>/` folders containing the controller, DTOs and use-case logic. No MediatR: a handler is a plain class registered in DI.

## Consequences
- Changing a feature touches one folder.
- If the logic grows, a `WordWise.Domain` project can be extracted (new ADR).
- Discipline: the controller stays thin, the logic lives in the feature class.

## Learning resources
- Controllers and attribute routing: https://learn.microsoft.com/aspnet/core/web-api/
- Dependency Injection and lifetimes: https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection
- Vertical Slice Architecture (Jimmy Bogard): https://www.jimmybogard.com/vertical-slice-architecture/
