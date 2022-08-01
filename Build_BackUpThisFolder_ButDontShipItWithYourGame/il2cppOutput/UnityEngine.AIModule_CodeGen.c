#include "pch-c.h"
#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include "codegen/il2cpp-codegen-metadata.h"





// 0x00000001 System.Void UnityEngine.AI.NavMeshAgent::set_destination(UnityEngine.Vector3)
extern void NavMeshAgent_set_destination_m5F0A8E4C8ED93798D6B9CE496B10FCE5B7461B95 (void);
// 0x00000002 UnityEngine.Vector3 UnityEngine.AI.NavMeshAgent::get_velocity()
extern void NavMeshAgent_get_velocity_m028219D0E4678D727F00C53AE3DCBCF29AF04DA7 (void);
// 0x00000003 System.Single UnityEngine.AI.NavMeshAgent::get_remainingDistance()
extern void NavMeshAgent_get_remainingDistance_m051C1B408E2740A95B5A5577C5EC7222311AA73A (void);
// 0x00000004 System.Boolean UnityEngine.AI.NavMeshAgent::SamplePathPosition(System.Int32,System.Single,UnityEngine.AI.NavMeshHit&)
extern void NavMeshAgent_SamplePathPosition_m6D4FD92B8727871A131A1E20AF0F3EDE20AE694A (void);
// 0x00000005 System.Void UnityEngine.AI.NavMeshAgent::set_speed(System.Single)
extern void NavMeshAgent_set_speed_m820E45289B3AE7DEE16F2F4BF163EAC361E64646 (void);
// 0x00000006 System.Void UnityEngine.AI.NavMeshAgent::set_destination_Injected(UnityEngine.Vector3&)
extern void NavMeshAgent_set_destination_Injected_m7195764B7610A893730EB50F1D9EB70BCDE65BD8 (void);
// 0x00000007 System.Void UnityEngine.AI.NavMeshAgent::get_velocity_Injected(UnityEngine.Vector3&)
extern void NavMeshAgent_get_velocity_Injected_m40A3E476CECB49AE84CA70761FB01FB5644B1735 (void);
// 0x00000008 System.Int32 UnityEngine.AI.NavMeshHit::get_mask()
extern void NavMeshHit_get_mask_m90866159EFD74EC854857ED33CBF1BA9BC04E927 (void);
// 0x00000009 System.Void UnityEngine.AI.NavMesh::Internal_CallOnNavMeshPreUpdate()
extern void NavMesh_Internal_CallOnNavMeshPreUpdate_m80148CFDD0C6F1DDDE5B3DA67A8D9613043A4233 (void);
// 0x0000000A System.Void UnityEngine.AI.NavMesh/OnNavMeshPreUpdate::.ctor(System.Object,System.IntPtr)
extern void OnNavMeshPreUpdate__ctor_m7142A3AA991BE50B637A16D946AB7604C64EF9BA (void);
// 0x0000000B System.Void UnityEngine.AI.NavMesh/OnNavMeshPreUpdate::Invoke()
extern void OnNavMeshPreUpdate_Invoke_mFB224B9BBF9C78B7F39AA91A047F175C69897914 (void);
static Il2CppMethodPointer s_methodPointers[11] = 
{
	NavMeshAgent_set_destination_m5F0A8E4C8ED93798D6B9CE496B10FCE5B7461B95,
	NavMeshAgent_get_velocity_m028219D0E4678D727F00C53AE3DCBCF29AF04DA7,
	NavMeshAgent_get_remainingDistance_m051C1B408E2740A95B5A5577C5EC7222311AA73A,
	NavMeshAgent_SamplePathPosition_m6D4FD92B8727871A131A1E20AF0F3EDE20AE694A,
	NavMeshAgent_set_speed_m820E45289B3AE7DEE16F2F4BF163EAC361E64646,
	NavMeshAgent_set_destination_Injected_m7195764B7610A893730EB50F1D9EB70BCDE65BD8,
	NavMeshAgent_get_velocity_Injected_m40A3E476CECB49AE84CA70761FB01FB5644B1735,
	NavMeshHit_get_mask_m90866159EFD74EC854857ED33CBF1BA9BC04E927,
	NavMesh_Internal_CallOnNavMeshPreUpdate_m80148CFDD0C6F1DDDE5B3DA67A8D9613043A4233,
	OnNavMeshPreUpdate__ctor_m7142A3AA991BE50B637A16D946AB7604C64EF9BA,
	OnNavMeshPreUpdate_Invoke_mFB224B9BBF9C78B7F39AA91A047F175C69897914,
};
extern void NavMeshHit_get_mask_m90866159EFD74EC854857ED33CBF1BA9BC04E927_AdjustorThunk (void);
static Il2CppTokenAdjustorThunkPair s_adjustorThunks[1] = 
{
	{ 0x06000008, NavMeshHit_get_mask_m90866159EFD74EC854857ED33CBF1BA9BC04E927_AdjustorThunk },
};
static const int32_t s_InvokerIndices[11] = 
{
	3761,
	4625,
	4571,
	776,
	3713,
	3589,
	3589,
	4505,
	7162,
	2007,
	4633,
};
IL2CPP_EXTERN_C const Il2CppCodeGenModule g_UnityEngine_AIModule_CodeGenModule;
const Il2CppCodeGenModule g_UnityEngine_AIModule_CodeGenModule = 
{
	"UnityEngine.AIModule.dll",
	11,
	s_methodPointers,
	1,
	s_adjustorThunks,
	s_InvokerIndices,
	0,
	NULL,
	0,
	NULL,
	0,
	NULL,
	NULL,
	NULL, // module initializer,
	NULL,
	NULL,
	NULL,
};
