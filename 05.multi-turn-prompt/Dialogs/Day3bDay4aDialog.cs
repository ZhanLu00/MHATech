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
    public class Day3bDialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;

        public Day3bDialog(UserState userState)
            : base(nameof(Day3bDialog))
        {
            _userProfileAccessor = userState.CreateProperty<UserProfile>("UserProfile");

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                BAP10001StepAsync,
                BAP10010StepAsync,
                BAP10013StepAsync
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
        private static async Task<DialogTurnResult> BAP10001StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Good morning!")
                , cancellationToken);

            Task.Delay(60000).Wait();

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Today we’ll be sending some messages about Behavioral Activation.")
                , cancellationToken);

            Random rnd1 = new Random();
            int skip_index_1 = rnd1.Next(0, 2); // generate a number in between 0 and 2

            switch (skip_index_1){
                case 0: // Gain Frame
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Behavioral Activation is a technique for boosting your mood by engaging in activities that you enjoy.")
                          , cancellationToken);
                    break;

                case 1: // Loss Frame
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Behavioral Activation is a technique for keeping low moods at bay by engaging in activities that you enjoy.")
                          , cancellationToken);
                    break;
            }

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Since doing enjoyable activities is rewarding, over time and with practice, you might find yourself doing them more and more. Click https://medicine.umich.edu/sites/default/files/content/downloads/Behavioral-Activation-for-Depression.pdf anytime to learn more about Behavioral Activation.")
                , cancellationToken);
            
            Task.Delay(480000).Wait();

            Random rnd2 = new Random();
            int skip_index_2 = rnd2.Next(0, 2);

            switch (skip_index_2){
                case 0: // Prompt(Directive)
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"The activities that people enjoy vary. It could be a walk, a favorite meal, stretching, taking a shower, writing, or something else. What comes to mind? \nMake a plan to do one pleasant activity later today.")
                          , cancellationToken);
                    break;

                case 1: // Prompt (NonDirective)
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"The activities that people enjoy vary. It could be a walk, a favorite meal, stretching, taking a shower, writing, or something else. What comes to mind? \nIf you want, you can try making a plan to do one pleasant activity later today.")
                          , cancellationToken);
                    break;
            }

            Task.Delay(180000).Wait();
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"✍ Write back the activity you are planning  ✍")
                , cancellationToken);
            
            Task.Delay(17820000).Wait();
            // 2:39pm
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Don’t forget to do a pleasant activity today.")
                , cancellationToken);

            Task.Delay(60000).Wait();


            Random rnd3 = new Random();
            int skip_index_3 = rnd3.Next(0, 3);

            switch (skip_index_3){

                case 0: // Affirmation
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"You deserve to enjoy yourself a bit. ")
                          , cancellationToken);
                    break;
                
                
                case 1: // Inspiration
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Today is what you make of it.")
                          , cancellationToken);
                    break;

                case 2: // Perspective
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Making changes to your routine can be hard, but it becomes easier over time.")
                          , cancellationToken);
                    break;
            }

            Task.Delay(21600000).Wait();
            stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ Were you able to do a pleasant activity today? Reply with “yes” or “no”  ✍") 
                    }, 
                    cancellationToken);
        }

        private static async Task<DialogTurnResult> BAP10010StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result){
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"That’s great! Congrats on taking that step!"), cancellationToken);
                
            } else {
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Maybe you can find an opportunity to do something enjoyable tomorrow! Take a few minutes now to think about what you’ll do and when you’ll do it."), cancellationToken);
            }
            Task.Delay(6300000).Wait();
            return await stepContext.Context.SendActivityAsync(MessageFactory.Text($"✍ Do you want to continue practicing Behavioral Activation tomorrow? Reply with 'yes' or 'no'  ✍"), cancellationToken);
        }

        private static async Task<DialogTurnResult> BAP10013StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result){
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                return await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Great! We’ll continue messaging about Behavioral Activation tomorrow!"), cancellationToken);
                
            } else {
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                return await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Sounds good! We’ll try something new."), cancellationToken);
            }
        }
        
    }
}
