// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;


namespace Microsoft.BotBuilderSamples
{
    public class Day2Dialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;

        public Day2Dialog(UserState userState)
            : base(nameof(Day13Dialog))
        {
            _userProfileAccessor = userState.CreateProperty<UserProfile>("UserProfile");

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                SCP10001StepAsync,
                SCP10009StepAsync,
                SCP10013StepAsync
            };

            // Add named dialogs to the DialogSet. These names are saved in the dialog state.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            // AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>), AgePromptValidatorAsync));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            // AddDialog(new AttachmentPrompt(nameof(AttachmentPrompt), PicturePromptValidatorAsync));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        // assume starts at 9:30 AM
        private static async Task<DialogTurnResult> SCP10001StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Good morning!")
                , cancellationToken);

            Task.Delay(60000).Wait();

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Today we’ll be sending some messages about Self-Compassion.")
                , cancellationToken);

            Random rnd1 = new Random();
            int skip_index_1 = rnd1.Next(0, 2); // generate a number in between 0 and 2

            switch (skip_index_1){
                case 0: // Gain Frame
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Self-compassion is a technique that involves acting kindly and gently towards oneself, even if/when we make mistakes or are feeling down. When we practice self-compassion, we give ourselves the benefit of the doubt.")
                          , cancellationToken);
                    break;

                case 1: // Loss Frame
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Self-compassion is a technique that involves acting kindly and gently towards oneself, even if/when we make mistakes or are feeling down. When we practice self-compassion, we choose not to judge ourselves harshly for our perceived flaws.")
                          , cancellationToken);
                    break;
            }

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Self-compassion involves responding in a non-judgmental, understanding, and caring way to oneself even when things are tough. Click https://self-compassion.org/the-three-elements-of-self-compassion-2/#definition anytime to learn more about Self-Compassion.")
                , cancellationToken);
            
            Task.Delay(480000).Wait();

            Random rnd2 = new Random();
            int skip_index_2 = rnd2.Next(0, 2);

            switch (skip_index_2){
                case 0: // Prompt(Directive)
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Mistakes are part of life. Today, see if you notice yourself thinking “I shouldn't have” or even “I was stupid.” If you notice those thoughts, imagine how you would comfort a friend in a similar situation.")
                          , cancellationToken);
                    Task.Delay(18000000).Wait();
                    break;

                case 1: // Prompt (NonDirective)
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Mistakes are part of life. Today, you might see if you notice yourself thinking “I shouldn't have” or even “I was stupid.” If you notice those thoughts, imagine how you would comfort a friend in a similar situation.")
                          , cancellationToken);
                    Task.Delay(18000000).Wait();
                    break;
            }

            Task.Delay(18000000).Wait();

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Don’t forget to be gentle on yourself today!")
                , cancellationToken);

            Task.Delay(60000).Wait();


            Random rnd3 = new Random();
            int skip_index_3 = rnd3.Next(0, 3);

            switch (skip_index_3){

                case 0: // Affirmation
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"You deserve kindness, even from yourself.")
                          , cancellationToken);
                    Task.Delay(14400000).Wait();
                    break;
                case 1: // Inspiration
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Today is a great day to go easy on yourself.")
                          , cancellationToken);
                    Task.Delay(14400000).Wait();
                    break;
                case 2: // Perspective
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Interrupting our self-critic and being gentler can be difficult, but it becomes easier over time.")
                          , cancellationToken);
                    Task.Delay(14400000).Wait();
                    break;
            }

            stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ Were you able to be gentler and more compassionate with yourself today? Reply with 'yes' or 'no' ✍") 
                    }, 
                    cancellationToken);
        }

        private static async Task<DialogTurnResult> SCP10009StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result){
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"That’s awesome! Congrats on taking that step!")
                          , cancellationToken);
                Task.Delay(180000).Wait();
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"✍ What's one way you could be even kinder to yourself?  ✍"), cancellationToken);
                Task.Delay(10000).Wait();
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for sharing."), cancellationToken);
            Task.Delay(6170000).Wait();
                
            } else {
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Maybe you will have an opportunity to practice self-compassion tomorrow or the next day."), cancellationToken);
                Task.Delay(6360000).Wait();
            }

            stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ Do you want to continue practicing Self-Compassion tomorrow? Reply with 'yes' or 'no' ✍") 
                    }, 
                    cancellationToken);
        }
        private static async Task<DialogTurnResult> SCP10013StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result){
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"Great! We’ll continue messaging about Self-Compassion tomorrow!"), cancellationToken);
  
            } else {
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Sounds good! We'll try something new."), cancellationToken);
            }
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);        }

    
    }
}
