using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Speech.Synthesis;
using WebApplication4.Models;
using NAudio.Lame;
using NAudio.Wave;
using System.Globalization;

using System.Speech.AudioFormat;
using System.Threading;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // POST: Item_table/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Item_table item_table = db.Item_table.Find(id);
            db.Item_table.Remove(item_table);
            db.SaveChanges();
            return RedirectToAction("Auction");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Auction(double? newbid, int? Id)
        {
            var price = from m in db.Item_table
                        select m;
            if (newbid == null)
            {
                return View(price.ToList());
            }
            else
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Item_table f = (from data in db.Item_table where data.Id == Id select data).FirstOrDefault();
                        f.prod_cbid = (double)newbid;
                        db.SaveChanges();

                        dbContextTransaction.Commit();
                        var upInfo = from m in db.Item_table
                                    select m;
                        return View(upInfo.ToList());
                    }
                    catch (Exception /*ex*/)
                    {
                        return View(price.ToList());
                    }
                }
            }

        }

        [HttpPost,ActionName("AuctionPost")]
        [ValidateAntiForgeryToken]
        public ActionResult Auction(int? newId, int? Id)
        {
            var price = from m in db.Item_table
                        select m;
            if (Id == null)
            {
                return View(price.ToList());
            }
            else
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Item_table f = (from data in db.Item_table where data.Id == Id select data).FirstOrDefault();
                        f.prod_active = 0;
                        Item_table newact = null;
                        do
                        {
                            newact = (from data in db.Item_table where data.Id == newId select data).FirstOrDefault();
                            newId++;
                        } while (newact == null);
                        newact.prod_active = 1;
                        db.SaveChanges();

                        dbContextTransaction.Commit();
                        var upInfo = from m in db.Item_table
                                     select m;
                        return View(upInfo.ToList());
                    }
                    catch (Exception /*ex*/)
                    {
                        return View(price.ToList());
                    }
                }
            }

        }

        public ActionResult Auction(int? id)
        {
                var price = from m in db.Item_table
                            select m;
            return View(price.ToList());
        }

        public void AuctionUpdate()
        {

            // Initialize a new instance of the SpeechSynthesizer.
            SpeechSynthesizer synth = new SpeechSynthesizer();

            // Configure the audio output. 
            synth.SetOutputToDefaultAudioDevice();
            synth.Volume = 100;
            // Speak a string.
            synth.Speak("Going Once, Going Twice!  SOLD!!");   
            
        }

        public FileResult TextToMp3(string text)
        {
            //Primary memory stream for storing mp3 audio
            var mp3Stream = new MemoryStream();
            //Speech format
            var speechAudioFormatConfig = new SpeechAudioFormatInfo
            (samplesPerSecond: 8000, bitsPerSample: AudioBitsPerSample.Sixteen,
            channel: AudioChannel.Stereo);
            //Naudio's wave format used for mp3 conversion. 
            //Mirror configuration of speech config.
            var waveFormat = new WaveFormat(speechAudioFormatConfig.SamplesPerSecond,
            speechAudioFormatConfig.BitsPerSample, speechAudioFormatConfig.ChannelCount);
            try
            {
                //Build a voice prompt to have the voice talk slower 
                //and with an emphasis on words
                var prompt = new PromptBuilder
                { Culture = CultureInfo.CreateSpecificCulture("en-US") };
                prompt.StartVoice(prompt.Culture);
                prompt.StartSentence();
                prompt.StartStyle(new PromptStyle()
                { Emphasis = PromptEmphasis.Reduced, Rate = PromptRate.Slow });
                prompt.AppendText(text);
                prompt.EndStyle();
                prompt.EndSentence();
                prompt.EndVoice();

                //Wav stream output of converted text to speech
                using (var synthWavMs = new MemoryStream())
                {
                    //Spin off a new thread that's safe for an ASP.NET application pool.
                    var resetEvent = new ManualResetEvent(false);
                    ThreadPool.QueueUserWorkItem(arg =>
                    {
                        try
                        {
                            //initialize a voice with standard settings
                            var siteSpeechSynth = new SpeechSynthesizer();
                            //Set memory stream and audio format to speech synthesizer
                            siteSpeechSynth.SetOutputToAudioStream
                                (synthWavMs, speechAudioFormatConfig);
                            //build a speech prompt
                            siteSpeechSynth.Speak(prompt);
                        }
                        catch (Exception ex)
                        {
                            //This is here to diagnostic any issues with the conversion process. 
                            //It can be removed after testing.
                            Response.AddHeader
                            ("EXCEPTION", ex.GetBaseException().ToString());
                        }
                        finally
                        {
                            resetEvent.Set();//end of thread
                        }
                    });
                    //Wait until thread catches up with us
                    WaitHandle.WaitAll(new WaitHandle[] { resetEvent });
                    //Estimated bitrate
                    var bitRate = (speechAudioFormatConfig.AverageBytesPerSecond * 8);
                    //Set at starting position
                    synthWavMs.Position = 0;
                    //Be sure to have a bin folder with lame dll files in there. 
                    //They also need to be loaded on application start up via Global.asax file
                    using (var mp3FileWriter = new LameMP3FileWriter
                    (outStream: mp3Stream, format: waveFormat, bitRate: bitRate))
                        synthWavMs.CopyTo(mp3FileWriter);
                }
            }
            catch (Exception ex)
            {
                Response.AddHeader("EXCEPTION", ex.GetBaseException().ToString());
            }
            finally
            {
                //Set no cache on this file
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                //required for chrome and safari
                Response.AppendHeader("Accept-Ranges", "bytes");
                //Write the byte length of mp3 to the client
                Response.AddHeader("Content-Length",
                    mp3Stream.Length.ToString(CultureInfo.InvariantCulture));
            }
            //return the converted wav to mp3 stream to a byte array for a file download
            return File(mp3Stream.ToArray(), "audio/mp3");
        }


        public ActionResult PlayTextArea(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                text = "Type something in first";
            }
            return TextToMp3(text);
        }
    }
}