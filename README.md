# Monitorador de sites (Worker)

Criaremos um Worker que vai monitorar atravéz do ping se o site esta acessível naquele momento gerando log de interface.

> Nuget

Baixar o pacote Newtonsoft.Json

![image](https://user-images.githubusercontent.com/99044436/193315189-bbfda63d-1875-4be6-90fb-8da2f5b19582.png)

> Criar as classes

###### Criar "ServiceConfigurations"

```C#

public class ServiceConfigurations
{

    public string[]? Hosts { get; set; }

    public int Intervalo { get; set; }

}
    
```

+ Criar "ResultadoMonitoramento"

```C#

public class ResultadoMonitoramento
{
    public string? Horario { get; set; }
    public string? Host { get; set; }
    public string? Status { get; set; }
    public object? Exception { get; set

```

###### Editar as classes

+ Editar "Worker"

Editar o método construtor

```C#

public Worker(ILogger<Worker> logger,IConfiguration configuration)
{
     _logger = logger;
     _serviceConfigurations = new ServiceConfigurations();
     new ConfigureFromConfigurationOptions<ServiceConfigurations>(
         configuration.GetSection("ServiceConfigurations"))
             .Configure(_serviceConfigurations);
}
```

Editar o método "ExecuteAsync"

```C#

protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    while (!stoppingToken.IsCancellationRequested)
    {
        _logger.LogInformation("Worker executando em: {time}",
            DateTimeOffset.Now);

        foreach (string host in _serviceConfigurations.Hosts)
        {
            _logger.LogInformation(
                $"Verificando a disponibilidade do host {host}");

            var resultado = new ResultadoMonitoramento();
            resultado.Horario =
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            resultado.Host = host;

            // Verifica a disponibilidade efetuando um ping
            // no host que foi configurado em appsettings.json
            try
            {
                using (Ping p = new Ping())
                {
                    var resposta = p.Send(host);
                    resultado.Status = resposta.Status.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado.Status = "Exception";
                resultado.Exception = ex;
            }

            string jsonResultado =
                JsonConvert.SerializeObject(resultado);
            if (resultado.Exception == null)
                _logger.LogInformation(jsonResultado);
            else
                _logger.LogError(jsonResultado);
        }

        await Task.Delay(
            _serviceConfigurations.Intervalo, stoppingToken);
    }
}

```





