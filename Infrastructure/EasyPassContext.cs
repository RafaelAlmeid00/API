using Api.Domain;
using dotenv.net;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure;

public partial class EasyPassContext : DbContext
{
    private readonly IDictionary<string, string> _envVariables;
    public EasyPassContext(DbContextOptions<EasyPassContext> options)
        : base(options)
    {
        DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: false));
        _envVariables = DotEnv.Read();
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<BusRoute> BusRoutes { get; set; }

    public virtual DbSet<BusStop> BusStops { get; set; }

    public virtual DbSet<Buss> Busses { get; set; }

    public virtual DbSet<Bussines> Bussines { get; set; }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<DriverBus> DriverBus { get; set; }

    public virtual DbSet<ListCpf> ListCpfs { get; set; }

    public virtual DbSet<RequestCard> RequestCards { get; set; }

    public virtual DbSet<Sac> Sacs { get; set; }

    public virtual DbSet<SacMessage> SacMessages { get; set; }

    public virtual DbSet<TurnBus> TurnBus { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<ValidationCard> ValidationCards { get; set; }







    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;Database=EasyPass;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdmId).HasName("PK__admin__8D530BF4875F64E2");

            entity.ToTable("admin");

            entity.HasIndex(e => e.AdmEmail, "UQ__admin__8C734C9A3F267892").IsUnique();

            entity.Property(e => e.AdmId).HasColumnName("adm_id");
            entity.Property(e => e.AdmEmail)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("adm_email");
            entity.Property(e => e.AdmLevel).HasColumnName("adm_level");
            entity.Property(e => e.AdmNome)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("adm_nome");
            entity.Property(e => e.AdmSenha)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("adm_senha");
        });

        modelBuilder.Entity<BusRoute>(entity =>
        {
            entity.HasKey(e => e.RouteNum).HasName("PK__bus_rout__C0525EBE811B6876");

            entity.ToTable("bus_route");

            entity.Property(e => e.RouteNum)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("route_num");
            entity.Property(e => e.RouteNome)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("route_nome");
        });

        modelBuilder.Entity<BusStop>(entity =>
        {
            entity.HasKey(e => e.StopId).HasName("PK__bus_stop__86FBE182BC391271");

            entity.ToTable("bus_stop");

            entity.Property(e => e.StopId).HasColumnName("stop_id");
            entity.Property(e => e.StopEndCep)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("stop_endCEP");
            entity.Property(e => e.StopEndUf)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("stop_endUF");
            entity.Property(e => e.StopEndcidade)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("stop_endcidade");
            entity.Property(e => e.StopEndcomplemento)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("stop_endcomplemento");
            entity.Property(e => e.StopEndnum)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("stop_endnum");
            entity.Property(e => e.StopEndrua)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("stop_endrua");

            entity.HasMany(d => d.BusRouteRouteNums).WithMany(p => p.BusStopStops)
                .UsingEntity<Dictionary<string, object>>(
                    "Route",
                    r => r.HasOne<BusRoute>().WithMany()
                        .HasForeignKey("BusRouteRouteNum")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_routes_bus_route1"),
                    l => l.HasOne<BusStop>().WithMany()
                        .HasForeignKey("BusStopStopId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_routes_bus_stop1"),
                    j =>
                    {
                        j.HasKey("BusStopStopId", "BusRouteRouteNum").HasName("PK__routes__A6FEFF5A7CCF17F9");
                        j.ToTable("routes");
                        j.IndexerProperty<int>("BusStopStopId").HasColumnName("bus_stop_stop_id");
                        j.IndexerProperty<string>("BusRouteRouteNum")
                            .HasMaxLength(45)
                            .IsUnicode(false)
                            .HasColumnName("bus_route_route_num");
                    });
        });

        modelBuilder.Entity<Buss>(entity =>
        {
            entity.HasKey(e => e.BusId).HasName("PK__buss__6ACEF8EDB3303948");

            entity.ToTable("buss");

            entity.Property(e => e.BusId)
                .ValueGeneratedNever()
                .HasColumnName("bus_id");
            entity.Property(e => e.BusFabricacao)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("bus_fabricacao");
            entity.Property(e => e.BusModelo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("bus_modelo");
            entity.Property(e => e.BusNome)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("bus_nome");
            entity.Property(e => e.BusPlaca)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("bus_placa");
            entity.Property(e => e.BusRouteRouteNum)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("bus_route_route_num");
            entity.Property(e => e.BusStatus)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("bus_status");
            entity.Property(e => e.BusTarifa)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("bus_tarifa");
            entity.Property(e => e.BussinesBussCnpj)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("bussines_buss_CNPJ");

            entity.HasOne(d => d.BusRouteRouteNumNavigation).WithMany(p => p.Busses)
                .HasForeignKey(d => d.BusRouteRouteNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_buss_bus_route1");

            entity.HasOne(d => d.BussinesBussCnpjNavigation).WithMany(p => p.Busses)
                .HasForeignKey(d => d.BussinesBussCnpj)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_buss_bussines1");
        });

        modelBuilder.Entity<Bussines>(entity =>
        {
            entity.HasKey(e => e.BussCnpj).HasName("PK__bussines__D3F085218EDC705C");

            entity.ToTable("bussines");

            entity.HasIndex(e => e.BussEmail, "UQ__bussines__F74F0E3464937711").IsUnique();

            entity.Property(e => e.BussCnpj)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("buss_CNPJ");
            entity.Property(e => e.BussContato)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("buss_contato");
            entity.Property(e => e.BussEmail)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("buss_email");
            entity.Property(e => e.BussEndCep)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("buss_endCEP");
            entity.Property(e => e.BussEndUf)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("buss_endUF");
            entity.Property(e => e.BussEndcidade)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("buss_endcidade");
            entity.Property(e => e.BussEndbairro)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("buss_endbairro");
            entity.Property(e => e.BussEndcomplemento)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("buss_endcomplemento");
            entity.Property(e => e.BussEndnum)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("buss_endnum");
            entity.Property(e => e.BussEndrua)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("buss_endrua");
            entity.Property(e => e.BussFotoPerfil).HasColumnName("buss_FotoPerfil");
            entity.Property(e => e.BussNome)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("buss_nome");
            entity.Property(e => e.BussSenha)
                .HasMaxLength(65)
                .IsUnicode(false)
                .HasColumnName("buss_senha");
            entity.Property(e => e.BussTipo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("buss_tipo");
            entity.Property(e => e.BussStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("buss_status");
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PK__card__BDF201DDB2F02FA4");

            entity.ToTable("card");

            entity.Property(e => e.CardId)
                .ValueGeneratedNever()
                .HasColumnName("card_id");
            entity.Property(e => e.CardSaldo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("card_saldo");
            entity.Property(e => e.CardStatus)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("card_status");
            entity.Property(e => e.CardTipo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("card_tipo");
            entity.Property(e => e.CardUltimoOnibus).HasColumnName("card_UltimoOnibus");
            entity.Property(e => e.CardUltimoUso)
                .HasColumnType("datetime")
                .HasColumnName("card_UltimoUso");
            entity.Property(e => e.CardValidade)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("card_validade");
            entity.Property(e => e.RequestCardReqId).HasColumnName("request_card_req_id");

            entity.HasOne(d => d.RequestCardReq).WithMany(p => p.Cards)
                .HasForeignKey(d => d.RequestCardReqId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_card_request_card1");
        });

        modelBuilder.Entity<DriverBus>(entity =>
        {
            entity.HasKey(e => e.DriverCpf).HasName("PK__driver_b__C25BBA67FAF275CC");

            entity.ToTable("driver_bus");

            entity.HasIndex(e => e.DriverRg, "UQ__driver_b__A41630E61CBDC4B0").IsUnique();

            entity.Property(e => e.DriverCpf)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("driver_CPF");
            entity.Property(e => e.DriverAdmissao).HasColumnName("driver_admissao");
            entity.Property(e => e.DriverDemissao).HasColumnName("driver_demissao");
            entity.Property(e => e.DriverNascimento).HasColumnName("driver_nascimento");
            entity.Property(e => e.DriverNome)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("driver_nome");
            entity.Property(e => e.DriverRg)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("driver_RG");
        });

        modelBuilder.Entity<ListCpf>(entity =>
        {
            entity.HasKey(e => e.ListId).HasName("PK__list_CPF__7B9EF135C1DEF0A8");

            entity.ToTable("list_CPF");

            entity.Property(e => e.ListId).HasColumnName("list_id");
            entity.Property(e => e.BussinesBussCnpj)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("bussines_buss_CNPJ");
            entity.Property(e => e.ListTipo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("list_tipo");

            entity.HasOne(d => d.BussinesBussCnpjNavigation).WithMany(p => p.ListCpfs)
                .HasForeignKey(d => d.BussinesBussCnpj)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_list_CPF_bussines");
        });

        modelBuilder.Entity<RequestCard>(entity =>
        {
            entity.HasKey(e => e.ReqId).HasName("PK__request___1513A6FBB34490F7");

            entity.ToTable("request_card");

            entity.Property(e => e.ReqId).HasColumnName("req_id");
            entity.Property(e => e.ReqData).HasColumnName("req_data");
            entity.Property(e => e.ReqEnvio).HasColumnName("req_envio");
            entity.Property(e => e.ReqTipoCartao)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("req_TipoCartao");
            entity.Property(e => e.UserUserCpf)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("user_user_CPF");

            entity.HasOne(d => d.UserUserCpfNavigation).WithMany(p => p.RequestCards)
                .HasForeignKey(d => d.UserUserCpf)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_request_card_user1");
        });

        modelBuilder.Entity<Sac>(entity =>
        {
            entity.HasKey(e => e.SacTicket).HasName("PK__sac__E9160A6518DD8B92");

            entity.ToTable("sac");

            entity.Property(e => e.SacTicket)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("sac_ticket");
            entity.Property(e => e.SacData).HasColumnName("sac_data");
            entity.Property(e => e.UserUserCpf)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("user_user_CPF");

            entity.HasOne(d => d.UserUserCpfNavigation).WithMany(p => p.Sacs)
                .HasForeignKey(d => d.UserUserCpf)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_SAC_USER_CPF1");
        });

        modelBuilder.Entity<SacMessage>(entity =>
        {
            entity.HasKey(e => e.SacmenId).HasName("PK__sac_mess__FA0CF17605AAD4F4");

            entity.ToTable("sac_message");

            entity.Property(e => e.SacmenId).HasColumnName("sacmen_id");
            entity.Property(e => e.AdminAdmId).HasColumnName("admin_adm_id");
            entity.Property(e => e.SacSacTicket)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("sac_sac_ticket");
            entity.Property(e => e.SacmenTexto)
                .HasColumnType("text")
                .HasColumnName("sacmen_texto");
            entity.Property(e => e.UserUserCpf)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("user_user_CPF");

            entity.HasOne(d => d.AdminAdm).WithMany(p => p.SacMessages)
                .HasForeignKey(d => d.AdminAdmId)
                .HasConstraintName("fk_sac_message_admin1");

            entity.HasOne(d => d.SacSacTicketNavigation).WithMany(p => p.SacMessages)
                .HasForeignKey(d => d.SacSacTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sac_message_sac1");

            entity.HasOne(d => d.UserUserCpfNavigation).WithMany(p => p.SacMessages)
                .HasForeignKey(d => d.UserUserCpf)
                .HasConstraintName("fk_sac_message_user1");
        });

        modelBuilder.Entity<TurnBus>(entity =>
        {
            entity.HasKey(e => e.TurnId).HasName("PK__turn_bus__40E36830EA72A8D8");

            entity.ToTable("turn_bus");

            entity.Property(e => e.TurnId).HasColumnName("turn_id");
            entity.Property(e => e.BussBusId).HasColumnName("buss_bus_id");
            entity.Property(e => e.DriverBusDriverCpf)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("driver_bus_driver_CPF");
            entity.Property(e => e.TurnFim)
                .HasColumnType("datetime")
                .HasColumnName("turn_fim");
            entity.Property(e => e.TurnInicio)
                .HasColumnType("datetime")
                .HasColumnName("turn_inicio");

            entity.HasOne(d => d.BussBus).WithMany(p => p.TurnBus)
                .HasForeignKey(d => d.BussBusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_turn_bus_buss1");

            entity.HasOne(d => d.DriverBusDriverCpfNavigation).WithMany(p => p.TurnBus)
                .HasForeignKey(d => d.DriverBusDriverCpf)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_turn_bus_driver_bus1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserCpf).HasName("PK__user__E3FC2463F2145C89");

            entity.ToTable("user");

            entity.Property(e => e.UserCpf)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("user_CPF");
            entity.Property(e => e.ListCpfListId).HasColumnName("list_CPF_list_id");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("user_email");
            entity.Property(e => e.UserEndCep)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("user_endCEP");
            entity.Property(e => e.UserEndUf)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("user_endUF");
            entity.Property(e => e.UserEndbairro)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("user_endbairro");
            entity.Property(e => e.UserEndcidade)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("user_endcidade");
            entity.Property(e => e.UserEndcomplemento)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("user_endcomplemento");
            entity.Property(e => e.UserEndnum)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("user_endnum");
            entity.Property(e => e.UserEndrua)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("user_endrua");
            entity.Property(e => e.UserFotoPerfil).HasColumnName("user_FotoPerfil");
            entity.Property(e => e.UserNascimento).HasColumnName("user_nascimento");
            entity.Property(e => e.UserNome)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("user_nome");
            entity.Property(e => e.UserRg)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("user_RG");
            entity.Property(e => e.UserRgfrente).HasColumnName("user_RGFrente");
            entity.Property(e => e.UserRgtras).HasColumnName("user_RGTras");
            entity.Property(e => e.UserSenha)
                .HasMaxLength(65)
                .IsUnicode(false)
                .HasColumnName("user_senha");
            entity.Property(e => e.UserTipo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("user_tipo");
            entity.Property(e => e.UserStatus)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("user_status");

            entity.HasOne(d => d.ListCpfList).WithMany(p => p.Users)
                .HasForeignKey(d => d.ListCpfListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_list_CPF1");
        });

        modelBuilder.Entity<ValidationCard>(entity =>
        {
            entity.HasKey(e => e.ValId).HasName("PK__validati__76DDA60150CFB989");

            entity.ToTable("validation_card");

            entity.Property(e => e.ValId).HasColumnName("val_id");
            entity.Property(e => e.CardCardId).HasColumnName("card_card_id");
            entity.Property(e => e.TurnBusTurnId).HasColumnName("turn_bus_turn_id");
            entity.Property(e => e.ValGasto)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("val_gasto");
            entity.Property(e => e.ValHorario)
                .HasColumnType("datetime")
                .HasColumnName("val_horario");
            entity.Property(e => e.ValOnibus).HasColumnName("val_onibus");

            entity.HasOne(d => d.CardCard).WithMany(p => p.ValidationCards)
                .HasForeignKey(d => d.CardCardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_validation_card_card1");

            entity.HasOne(d => d.TurnBusTurn).WithMany(p => p.ValidationCards)
                .HasForeignKey(d => d.TurnBusTurnId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_validation_card_turn_bus1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
