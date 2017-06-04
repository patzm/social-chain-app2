﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PartyTimeline
{
	/// <summary>
	/// RestClient implements methods for calling CRUD operations
	/// using HTTP.
	/// </summary>
	public abstract class RestClient<T>
	{
		protected HttpClient httpClient;

		private JsonSerializerSettings serializationSettings;

		protected string endpoint = string.Empty;

        protected const string sep = "/";

        protected const string serverBaseUrl = "http://lowcost-env.zk8xjtydiz.us-west-2.elasticbeanstalk.com";
		protected const string appName = "partytimeline";
		protected const string appApiNode = "api";
		protected const string apiVersion = "v1";
		protected string serverUrl;

		public RestClient(string endpoint)
		{
			this.endpoint = endpoint;
			serverUrl = UrlJoin(serverBaseUrl, appName, appApiNode, apiVersion, this.endpoint);
			Debug.WriteLine($"Using server URL {serverUrl} for type {this.GetType().ToString()}");
			httpClient = new HttpClient();
			serializationSettings = new JsonSerializerSettings()
			{
				DateTimeZoneHandling = DateTimeZoneHandling.Utc
			};
		}

		public async Task<List<T>> GetAsync(string custom_endpoint = null)
		{
			var json = await httpClient.GetStringAsync(UrlJoin(serverUrl, custom_endpoint));

			var taskModels = JsonConvert.DeserializeObject<List<T>>(json, serializationSettings);

			return taskModels;
		}

		public async Task<bool> PostAsync(T t, string custom_endpoint = null)
		{
			var json = JsonConvert.SerializeObject(t, serializationSettings);
			Debug.WriteLine($"POST: Serialized object for {this.GetType().ToString()}: {json}");
			HttpContent httpContent = new StringContent(json);

			httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var result = await httpClient.PostAsync(UrlJoin(serverUrl, custom_endpoint), httpContent);
            LogResponse(result);
			return result.IsSuccessStatusCode;
		}

		public async Task<bool> PutAsync(int id, T t, string custom_endpoint = null)
		{
			var json = JsonConvert.SerializeObject(t, serializationSettings);

			HttpContent httpContent = new StringContent(json);

			httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var result = await httpClient.PutAsync(UrlJoin(serverUrl, custom_endpoint, id), httpContent);
            LogResponse(result);
            return result.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteAsync(int id, T t, string custom_endpoint = null)
		{
			var result = await httpClient.DeleteAsync(UrlJoin(serverUrl, custom_endpoint, id));
            LogResponse(result);

            return result.IsSuccessStatusCode;
        }

        public static string UrlJoin(params object[] parts)
		{
			return string.Join(sep, parts.Where((arg) => arg != null).Select<object, string>((part) =>
            {
                string s = part.ToString();
                s = s.TrimStart(sep.ToCharArray());
                s = s.TrimEnd(sep.ToCharArray());
                    
                return s;
            }
            )) + sep;
		}

        protected void LogResponse(HttpResponseMessage msg)
        {string log_sep = "\n#####\n";
            Debug.WriteLine($"{log_sep}Response:\nStatusCode = {msg.StatusCode}\nRequestMessage = {msg.RequestMessage.ToString().Replace("\n", " ")}\nContent = {msg.Content}{log_sep}");
        }
	}
}
