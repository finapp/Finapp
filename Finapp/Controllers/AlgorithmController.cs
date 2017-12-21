using Finapp.ICreateDatabase;
using Finapp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class AlgorithmController : Controller
    {
        private readonly Func<IAlgorithms> _algorithmFactory;
        private readonly ICreator _creator;

        public AlgorithmController(Func<IAlgorithms> algorithmFactory, ICreator creator)
        {
            _algorithmFactory = algorithmFactory;
            _creator = creator;
        }

        public ActionResult Index()
        {
            Task.Run(() => _algorithmFactory.Invoke().Associating());

            return RedirectToAction("Index", "Creditor");
        }

        public ActionResult Test10()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    _algorithmFactory.Invoke().Associating();
                }
            });


            return RedirectToAction("Index", "Creditor");
        }

        public ActionResult Test65()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 65; i++)
                {
                    _algorithmFactory.Invoke().Associating();
                }
            });

            return RedirectToAction("Index", "Creditor");
        }


        public ActionResult Test100()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    _algorithmFactory.Invoke().Associating();
                }
            });


            return RedirectToAction("Index", "Creditor");
        }

        public ActionResult Test365()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 365; i++)
                {
                    _algorithmFactory.Invoke().Associating();
                }
            });
            return RedirectToAction("Index", "Creditor");
        }
    }
}