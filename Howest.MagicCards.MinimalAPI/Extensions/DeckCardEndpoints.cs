using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO.DeckDTO;


namespace Howest.MagicCards.MinimalAPI.Extensions;

public static class DeckCardEndpoints
{
    public static void MapDeckCardEndpoints(this RouteGroupBuilder cardDeckGroup, string urlPrefix)
    {
        const string tag = "DeckEntries";

        cardDeckGroup.MapGet("", async (IDeckRepository cardDeckRepo, IMapper mapper) =>
        {
            IEnumerable<DeckEntry> deckCards = await cardDeckRepo.GetDeckEntriesAsync();
            IEnumerable<DeckEntryReadDTO> result = mapper.Map<IEnumerable<DeckEntryReadDTO>>(deckCards);
            return Results.Ok(result);
        }).WithTags(tag)
          .WithName("GetAllDeckEntries")
          .Produces<IEnumerable<DeckEntryReadDTO>>(StatusCodes.Status200OK);


        cardDeckGroup.MapGet("/{EntryId}", async (IDeckRepository cardDeckRepo, IMapper mapper, string EntryId) =>
        {
            DeckEntry deckEntry = await cardDeckRepo.GetDeckEntryByIdAsync(EntryId);
            if (deckEntry is null)
            {
                return Results.NotFound($"No deck entry found with id {EntryId}");
            }
            DeckEntryReadDTO result = mapper.Map<DeckEntryReadDTO>(deckEntry);
            return Results.Ok(result);
        }).WithTags(tag)
          .WithName("GetDeckEntryById")
          .Produces<DeckEntryReadDTO>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status404NotFound);


        cardDeckGroup.MapGet("/cards/{cardId}", async (IDeckRepository cardDeckRepo, IMapper mapper, long cardId) =>
        {
            DeckEntry deckCard = await cardDeckRepo.GetDeckCardByCardIdAsync(cardId);
            if (deckCard is null)
            {
                return Results.NotFound($"No deck card found with card id {cardId}");
            }
            DeckEntryReadDTO result = mapper.Map<DeckEntryReadDTO>(deckCard);
            return Results.Ok(result);
        }).WithTags(tag)
          .WithName("GetDeckEntryByCardId")
          .Produces<DeckEntryReadDTO>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status404NotFound);



        cardDeckGroup.MapPost("", async (IDeckRepository cardDeckRepo, IValidator<DeckEntryWriteDTO> validator, DeckEntryWriteDTO newDeckCard, IMapper mapper) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(newDeckCard);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }


            DeckCard deckCardToCreate = mapper.Map<DeckCard>(newDeckCard);

            DeckEntry deckCard = await cardDeckRepo.GetDeckCardByCardIdAsync(deckCardToCreate.Id);
            if (deckCard != null)
            {
                deckCard.Quantity++;
                cardDeckRepo.UpdateDeckEntryAsync(deckCard);
                return Results.Ok($"Deck card with card id {deckCardToCreate.Id} already exists. Quantity increased by 1.");
            }

            DeckEntry newDeckEntry = mapper.Map<DeckEntry>(newDeckCard);
            newDeckEntry = new DeckEntry
            {
                EntryId = newDeckEntry.EntryId ?? $"deckEntry:{Guid.NewGuid()}",
                Card = deckCardToCreate,
                Quantity = newDeckEntry.Quantity == 0 ? 1 : newDeckEntry.Quantity
            };
            cardDeckRepo.AddDeckEntryAsync(newDeckEntry);

            DeckEntryReadDTO result = mapper.Map<DeckEntryReadDTO>(newDeckEntry);
            return Results.Created($"{urlPrefix}/deckCards/{result.EntryId}", result);
        }).WithTags(tag)
          .WithName("CreateDeckEntry")
          .Produces<DeckEntryReadDTO>(StatusCodes.Status201Created)
          .Produces(StatusCodes.Status400BadRequest);


        cardDeckGroup.MapPut("/cards/{cardId}", async (IDeckRepository cardDeckRepo, IValidator<DeckEntryWriteDTO> validator, long cardId, DeckEntryWriteDTO updatedDeckCard, IMapper mapper) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(updatedDeckCard);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            DeckEntry existingDeckEntry = await cardDeckRepo.GetDeckCardByCardIdAsync(cardId);
            if (existingDeckEntry == null)
            {
                return Results.NotFound($"No deck card found with card id {cardId}");
            }

            mapper.Map(updatedDeckCard, existingDeckEntry.Card);
            cardDeckRepo.UpdateDeckEntryAsync(existingDeckEntry);

            DeckEntryReadDTO result = mapper.Map<DeckEntryReadDTO>(existingDeckEntry);
            return Results.Ok(result);
        }).WithTags(tag)
          .WithName("UpdateDeckEntryByCardId")
          .Produces<DeckEntryReadDTO>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status400BadRequest)
          .Produces(StatusCodes.Status404NotFound);


        cardDeckGroup.MapPatch("/cards/{cardId}/increase", async (IDeckRepository cardDeckRepo, long cardId, IMapper mapper) =>
        {
            DeckEntry deckCard = await cardDeckRepo.GetDeckCardByCardIdAsync(cardId);

            if (deckCard == null)
            {
                return Results.NotFound($"No deck card found with card id {cardId}");
            }

            deckCard.Quantity++;
            cardDeckRepo.UpdateDeckEntryAsync(deckCard);
            return Results.NoContent();
        }).WithTags(tag)
          .WithName("IncreaseDeckEntryQuantity")
          .Produces(StatusCodes.Status204NoContent)
          .Produces(StatusCodes.Status404NotFound);



        cardDeckGroup.MapPatch("/cards/{cardId}/decrease", async (IDeckRepository cardDeckRepo, long cardId, IMapper mapper) =>
        {
            DeckEntry deckCard = await cardDeckRepo.GetDeckCardByCardIdAsync(cardId);

            if (deckCard is null)
            {
                return Results.NotFound($"No deck card found with card id {cardId}");
            }

            if (deckCard.Quantity == 1)
            {
                cardDeckRepo.DeleteDeckEntryAsync(deckCard);
                return Results.NoContent();
            }

            deckCard.Quantity--;
            cardDeckRepo.UpdateDeckEntryAsync(deckCard);
            return Results.NoContent();
        }).WithTags(tag)
          .WithName("DecreaseDeckEntryQuantity")
          .Produces(StatusCodes.Status204NoContent)
          .Produces(StatusCodes.Status404NotFound);


        cardDeckGroup.MapDelete("/cards/{cardId}", async (IDeckRepository cardDeckRepo, long cardId, IMapper mapper) =>
        {
            DeckEntry deckCard = await cardDeckRepo.GetDeckCardByCardIdAsync(cardId);

            if (deckCard is null)
            {
                return Results.NotFound($"No deck card found with card id {cardId}");
            }

            cardDeckRepo.DeleteDeckEntryAsync(deckCard);
            return Results.NoContent();
        }).WithTags(tag)
          .WithName("DeleteDeckEntryByCardId")
          .Produces(StatusCodes.Status204NoContent)
          .Produces(StatusCodes.Status404NotFound);


        cardDeckGroup.MapDelete("", (IDeckRepository cardDeckRepo) =>
        {
            cardDeckRepo.DeleteAllEntriesAsync();
            return Results.NoContent();
        }).WithTags(tag)
          .WithName("DeleteAllDeckEntries")
          .Produces(StatusCodes.Status204NoContent);
    }
}

