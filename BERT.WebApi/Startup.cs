﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ML.Models.BERT;

namespace BERT.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Enable Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddSingleton(o =>
            {
                var modelConfig = new BertModelConfiguration()
                {
                    VocabularyFile = "Model/vocab.txt",
                    ModelPath = "Model/bertsquad-10.onnx"
                };

                var model = new BertModel(modelConfig);
                model.Initialize();

                return model;
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenAI API v1");
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(e =>
            {
                e.MapControllers();
            });
        }
    }
}