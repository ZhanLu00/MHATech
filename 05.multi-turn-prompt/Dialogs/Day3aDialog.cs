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
    public class Day3aDialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;

        public Day3aDialog(UserState userState)
            : base(nameof(Day3aDialog))
        {
            _userProfileAccessor = userState.CreateProperty<UserProfile>("UserProfile");

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                SCP20001StepAsync,
                SCP20009StepAsync
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
        private static async Task<DialogTurnResult> SCP20001StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Good morning!")
                , cancellationToken);

            Task.Delay(60000).Wait();

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Today we’ll be sending more messages about Self-Compassion.")
                , cancellationToken);

            Random rnd1 = new Random();
            int skip_index_1 = rnd1.Next(0, 2); // generate a number in between 0 and 2

            switch (skip_index_1){
                case 0: // Gain Frame
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Self-compassion involves the way that we respond to ourselves when things are difficult. Sometimes we use overly harsh language with ourselves when we’ve made mistakes or didn’t achieve a goal. When we make an effort to shift our language to be kinder and gentler, we may find it easier to forgive ourselves.")
                          , cancellationToken);
                    break;

                case 1: // Loss Frame
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Self-compassion involves the way that we respond to ourselves when things are difficult. Sometimes we use overly harsh language with ourselves when we’ve made mistakes or didn’t achieve a goal. When we make an effort to shift that language to be kinder and gentler, we may notice fewer low moods and a quieter self-critic.")
                          , cancellationToken);
                    break;
            }

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"To learn more about how to practice self-compassion and to use “forgiving language” click here: https://positivepsychology.com/how-to-practice-self-compassion/")
                , cancellationToken);
            
            Task.Delay(480000).Wait();

            Random rnd2 = new Random();
            int skip_index_2 = rnd2.Next(0, 2);

            switch (skip_index_2){
                case 0: // Prompt(Directive)
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Like other new skills, self-compassion takes practice and patience. The next time you find yourself being hard on yourself or wishing you would have handled something differently, notice the language you use. Ask yourself, “How might I change this language if I were speaking to a friend?”")
                          , cancellationToken);
                    break;

                case 1: // Prompt (NonDirective)
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Like other new skills, self-compassion takes practice and patience. The next time you find yourself being hard on yourself or wishing you would have handled something differently, you might try to notice the language you use. You could ask, “how might I change this language if I were speaking to a friend?”")
                          , cancellationToken);
                    break;
            }

            Task.Delay(18000000).Wait();
            // 2:39pm
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Don’t forget to try to use compassionate language today.")
                , cancellationToken);

            Task.Delay(60000).Wait();


            Random rnd3 = new Random();
            int skip_index_3 = rnd3.Next(0, 3);

            switch (skip_index_3){

                case 0: // Affirmation
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"You are as capable of self-compassion as anyone else.")
                          , cancellationToken);
                    Task.Delay(18000000).Wait();
                    break;
                
                
                case 1: // Inspiration
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Today is a new day to try new things.")
                          , cancellationToken);
                    Task.Delay(18000000).Wait();
                    break;

                case 2: // Perspective
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"If you struggle to use compassionate language towards yourself, don’t worry. It can be a challenge. Start by noticing your language, without judgement.")
                          , cancellationToken);
                    Task.Delay(18000000).Wait();
                    break;
            }

            stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ Were you able to practice self-compassion today? Reply with “yes” or “no”  ✍") 
                    }, 
                    cancellationToken);
        }

        private static async Task<DialogTurnResult> SCP20009StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result){
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"That’s great! Seems like the work you are doing is paying off.")
                          , cancellationToken);
                Task.Delay(60000).Wait();

                Random rnd4 = new Random();
                int skip_index_4 = rnd4.Next(0, 3);
                switch (skip_index_4){
                    case 0: 
                        await stepContext.Context.SendActivityAsync(MessageFactory.Text($"✍ What's one compassionate thing you can say to yourself right now?  ✍"), cancellationToken);
                        break;
                    case 1: 
                        await stepContext.Context.SendActivityAsync(MessageFactory.Text($"✍ How do you feel when you're compassionate to yourself?  ✍"), cancellationToken);
                        break;
                    case 2: 
                        await stepContext.Context.SendActivityAsync(MessageFactory.Text($"✍ How do you feel about practicing self-compassion?  ✍"), cancellationToken);
                        break;

                }

                Task.Delay(10000).Wait();
                return await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for sharing."), cancellationToken);
                
            } else {
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                return await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Maybe you will have an opportunity to practice self-compassion tomorrow or the next day."), cancellationToken);
            }
        }
    }
}
