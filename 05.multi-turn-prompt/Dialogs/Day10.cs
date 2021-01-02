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
    public class Day10 : ComponentDialog
    {
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;

        public Day10(UserState userState)
            : base(nameof(Day10))
        {
            _userProfileAccessor = userState.CreateProperty<UserProfile>("UserProfile");

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                StepS20001ToS20005, // includes S20001, S20002, S20003, S20004, S20005
                StepS20006ToS20011, // includes S20006, S20007, S20008, S20009, S20010, S20011
                StepS20012ToS20016,
                StepS20017,
                StepS20018ToS20020,
                StepS20021ToS20024,
                StepS20025ToS20028,
                StepS20029,
                StepS20030ToS20032,
                StepS20033ToS20036,
                StepS20037ToS20040,
                StepS20041,
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

        // Steps
        private static async Task<DialogTurnResult> StepS20001ToS20005(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // TODO: Text should be "Good morning, [name]."
            await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Good morning!")
                        , cancellationToken);
            
            await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Here's an experience from another person using this texting program. They were feeling overwhelmed trying to live up to other people's expectations:")
                        , cancellationToken);

            await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Going to college was a new and exciting experience, but at the same time, I  dreaded it. It wasn't so much being away from home or navigating new social dynamics — though both were pretty intimidating.")
                        , cancellationToken);

            await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"For me, the thing I dreaded was the weight of my parents' judgment on my back. For as long as I can remember, I've always wanted my parents to be proud of me. But they didn't approve of my interests. I wanted to major in art, but they thought it impractical.")
                        , cancellationToken);
            
            return await stepContext.PromptAsync(nameof(ChoicePrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Text \"M\" to hear more or text \"D\" to hear a different story ✍"),
                    Choices = ChoiceFactory.ToChoices(new List<string> {"M", "D"}),
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> StepS20006ToS20011(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["S20005"] = ((FoundChoice)stepContext.Result).Value;

            if (stepContext.Values["S20005"].ToString() == "D") {
                // jump to StepS20018ToS20020
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"] + 2;
                return await StepS20018ToS20020(stepContext, cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"I knew where my parents were coming from. They wanted to make sure I was financially secure, that I'd be in a respectable profession. So, it makes sense that they'd want me to pursue a \"safe\" major."), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"In my first year, I found that I hated the classes my parents want me to take and loved my art electives. I’d been scared to tell them. I was sure that they wouldn’t understand my passion for art. I was also scared that they would be disappointed, angry, and even hurtful. If I were to talk to them, we would have a huge fight."), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"But I remembered something a friend said to me when I told them about my situation. They said, \"it's your life, you're the one that needs to live it.\" So, I told them my parents I wanted to switch my major to art, and that I'd been taking art classes."), cancellationToken);
            
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Can you relate to this person’s experience? Reply with “Yes” or “No” ✍")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> StepS20012ToS20016(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for the feedback!"), cancellationToken);
            
            if (!((bool)stepContext.Result)) {
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Sometimes, we think we know what's going to happen in certain situations, which can lead us to try to take the path of least resistance to avoid upsetting people."), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"This is a thinking trap called Fortune Telling. Fortune telling is when we believe we know exactly what's going to happen even though we haven't actually tried confronting the situation. Sometimes, our predictions are right, but sometimes we have to test them before we can really know what's going to happen."), cancellationToken);

            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Can you think of a time when you were \"fortune telling\" about how a situation would work out? ✍")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> StepS20017(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for sharing"), cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }


        private static async Task<DialogTurnResult> StepS20018ToS20020(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Here's a story from someone who has been feeling sad because they didn't end up getting a job they were really excited about."), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Last month after I got an email saying that I didn't get the job I interviewed for (I went through 3 rounds of interviews!), I was devastated. I thought to myself, \"I failed...again.\" After getting that rejection, I replayed the interviews over and over in my mind trying to figure out what I could have done better."), cancellationToken);

            return await stepContext.PromptAsync(nameof(ChoicePrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Text \"M\" to hear more or text \"D\" to hear a different story ✍"),
                    Choices = ChoiceFactory.ToChoices(new List<string> {"M", "D"}),
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> StepS20021ToS20024(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["S20020"] = ((FoundChoice)stepContext.Result).Value;

            if (stepContext.Values["S20020"].ToString() == "D") {
                // jump to StepS20030ToS20032
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"] + 2;
                return await StepS20030ToS20032(stepContext, cancellationToken);
            }


            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"A few days after getting that rejection email, my younger brother called me. He was finishing his last semester in college and was starting to interview for jobs too. He knew I had some experience interviewing, so he asked me for tips."), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"I thought a bit about this and then told him to practice with a friend or video record himself and watch the recordings. That way, he can get some practice and see how he's coming across. I also told him that I believe in him and he shouldn't take it personally no matter what happens."), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Then it hit me: why shouldn't the same advice apply to me? I'm still sad I didn't get that original job, but I can practice doing mock interviews with friends or recording my answers. And I know that whether or not I get the job doesn't define me, just like it wouldn't define my brother."), cancellationToken);
            
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Can you relate to this person’s experience? Reply with “Yes” or “No” ✍")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> StepS20025ToS20028(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for the feedback!"), cancellationToken);
            
            if (!((bool)stepContext.Result)) {
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"It's sometimes hard to extend the same generosity and kindness to ourselves that we show to others in our lives."), cancellationToken);
            
            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Is there a disappointment you faced that you were too hard on yourself about? ✍")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> StepS20029(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for sharing"), cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private static async Task<DialogTurnResult> StepS20030ToS20032(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Here's a story from someone feeling isolated during the Covid-19 pandemic:"), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"I know this year has been hard on everyone, so I feel bad even complaining about it. I am healthy, and I have some financial security, so I already am lucky in a way."), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"But it has been just really hard for me, especially living alone. I consider myself an introvert, so I never really thought that I would miss other people. I realize that I miss little interactions I used to have, like smiling or talking with the cashier when I get a coffee. Or just sitting near people and overhearing their conversations."), cancellationToken);

            // TODO: the spreadsheet only allows one option here. What should happen if the user does not want to hear more?
            return await stepContext.PromptAsync(nameof(ChoicePrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Text \"M\" to hear more ✍"),
                    Choices = ChoiceFactory.ToChoices(new List<string> {"M"}),
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> StepS20033ToS20036(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //stepContext.Values["S20030"] = ((FoundChoice)stepContext.Result).Value;
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"When I have gone on social media to try to have some human contact, it has backfired on me. I know other people are struggling like me. But on social media it seems like people are still only showing the positive side of their lives. It seems \"fake\" for lack of a better word."), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Speaking for myself, I have realized I need be more deliberate about my social media use, and not substitute it for having one-on-one interactions (even if those can't be in person right now)."), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"I am not usually one to talk on the phone, but I decided to call my best friend from college yesterday. We only talked for a couple minutes, but it was nice to hear a friendly voice and to joke around a bit. We made a plan to both watch a Netflix show on Friday and to text about it, which gives me something to look forward to at least."), cancellationToken);
            
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Can you relate to this person’s experience? Reply with “Yes” or “No” ✍")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> StepS20037ToS20040(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for the feedback!"), cancellationToken);
            
            if (!((bool)stepContext.Result)) {
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Sometimes we have to take the first step to reach out to people."), cancellationToken);

            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Is there a step you could take to connect to a person you care about? ✍")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> StepS20041(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for sharing"), cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

    }

}