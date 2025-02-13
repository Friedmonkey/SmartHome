//using SmartHome.Backend.FastEndpoints.Base;
//using SmartHome.Common;
//using SmartHome.Common.Api;
//using static SmartHome.Common.Api.ISmartHomeService;

//namespace SmartHome.Backend.FastEndpoints;

//public class CreateSmartHomeEndpoint : BasicEndpointBase<CreateSmartHomeRequest, SuccessResponse>
//{
//    public required ISmartHomeService SmartHomeService { get; set; }
//    public override void Configure()
//    {
//        Post(SharedConfig.Urls.SmartHome.AddSmartHomeUrl);
//        AllowAnonymous();
//    }

//    public override async Task HandleAsync(CreateSmartHomeRequest request, CancellationToken ct)
//    {
//        await SendAsync(await SmartHomeService.CreateSmartHome(request));
//    }
//}

//public class GetSmartHomesOfSmartUser : BasicEndpointBase<GuidRequest, SmartHomeResponse>
//{
//    public required ISmartHomeService SmartHomeService { get; set; }
//    public override void Configure()
//    {
//        Post(SharedConfig.Urls.SmartHome.AddSmartHomeUrl);
//        AllowAnonymous();
//    }

//    public override async Task HandleAsync(GuidRequest request, CancellationToken ct)
//    {
//        await SendAsync(await SmartHomeService.GetSmartHomesOfSmartUser(request));
//    }
//}

//public class UpdateSmartHomeEndpoint : BasicEndpointBase<UpdateSmartHomeRequest, SuccessResponse>
//{
//    public required ISmartHomeService SmartHomeService { get; set; }
//    public override void Configure()
//    {
//        Post(SharedConfig.Urls.SmartHome.UpdateSmartHomeUrl);
//        AllowAnonymous();
//    }

//    public override async Task HandleAsync(UpdateSmartHomeRequest request, CancellationToken ct)
//    {
//        await SendAsync(await SmartHomeService.UpdateSmartHome(request));
//    }
//}
//public class DeleteSmartHomeEndpoint : BasicEndpointBase<GuidRequest, SuccessResponse>
//{
//    public required ISmartHomeService SmartHomeService { get; set; }
//    public override void Configure()
//    {
//        Post(SharedConfig.Urls.SmartHome.DeleteSmartHomeUrl);
//        AllowAnonymous();
//    }

//    public override async Task HandleAsync(GuidRequest request, CancellationToken ct)
//    {
//        await SendAsync(await SmartHomeService.DeleteSmartHome(request));
//    }
//}