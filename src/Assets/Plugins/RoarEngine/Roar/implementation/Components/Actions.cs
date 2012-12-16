using System.Collections.Generic;
using Roar.Components;

namespace Roar.implementation.Components
{
	public class Actions : IActions
	{
		protected IDataStore dataStore;
		protected IWebAPI.ITasksActions taskActions;

		public Actions (IWebAPI.ITasksActions taskActions, IDataStore dataStore)
		{
			this.taskActions = taskActions;
			this.dataStore = dataStore;
		}

		public bool HasDataFromServer { get { return dataStore.actions.HasDataFromServer; } }

		public void Fetch (Roar.Callback< IDictionary<string,Roar.DomainObjects.Task> > callback)
		{
			dataStore.actions.Fetch (callback);
		}

		public IList<Roar.DomainObjects.Task> List ()
		{
			return dataStore.actions.List ();
		}

		public void Execute (string ikey, Roar.Callback<WebObjects.Tasks.StartResponse> callback)
		{
			WebObjects.Tasks.StartArguments args = new WebObjects.Tasks.StartArguments();
			args.task_ikey = ikey;
			taskActions.start (args, new OnActionsDo (callback));
		}
		class OnActionsDo : ZWebAPI.Callback<WebObjects.Tasks.StartResponse>
		{
			//Actions actions;
			Roar.Callback<WebObjects.Tasks.StartResponse> cb_;

			public OnActionsDo (Roar.Callback<WebObjects.Tasks.StartResponse> in_cb)
			{
				cb_ = in_cb;
			}

			public void OnError( Roar.RequestResult nn )
			{
				cb_( new CallbackInfo<WebObjects.Tasks.StartResponse>(null, nn.code, nn.msg) );
			}

			public void OnSuccess ( CallbackInfo<WebObjects.Tasks.StartResponse> info)
			{
				cb_(info);
			}
		}
	}

}
